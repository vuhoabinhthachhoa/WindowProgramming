package com.windowprogramming.ClothingStoreManager.service.invoice;

import com.windowprogramming.ClothingStoreManager.converter.invoice.InvoiceConverter;
import com.windowprogramming.ClothingStoreManager.dto.request.invoice.InvoiceCreationRequest;
import com.windowprogramming.ClothingStoreManager.dto.response.PageResponse;
import com.windowprogramming.ClothingStoreManager.dto.response.invoice.InvoiceAggregationResponse;
import com.windowprogramming.ClothingStoreManager.dto.response.invoice.InvoiceDetailResponse;
import com.windowprogramming.ClothingStoreManager.dto.response.invoice.InvoiceGeneralResponse;
import com.windowprogramming.ClothingStoreManager.entity.Invoice;
import com.windowprogramming.ClothingStoreManager.entity.InvoiceDetail;
import com.windowprogramming.ClothingStoreManager.entity.Product;
import com.windowprogramming.ClothingStoreManager.enums.SortType;
import com.windowprogramming.ClothingStoreManager.exception.AppException;
import com.windowprogramming.ClothingStoreManager.exception.ErrorCode;
import com.windowprogramming.ClothingStoreManager.mapper.EmployeeMapper;
import com.windowprogramming.ClothingStoreManager.repository.EmployeeRepository;
import com.windowprogramming.ClothingStoreManager.repository.InvoiceDetailRepository;
import com.windowprogramming.ClothingStoreManager.repository.InvoiceRepository;
import com.windowprogramming.ClothingStoreManager.repository.ProductRepository;
import com.windowprogramming.ClothingStoreManager.service.product.ProductService;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

@Service
@RequiredArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
public class InvoiceServiceImpl implements InvoiceService {
    ProductService productService;

    EmployeeRepository employeeRepository;
    ProductRepository productRepository;
    InvoiceRepository invoiceRepository;
    InvoiceDetailRepository invoiceDetailRepository;

    InvoiceConverter invoiceConverter;

    EmployeeMapper employeeMapper;

    @Override
    public InvoiceGeneralResponse createInvoice(InvoiceCreationRequest invoiceCreationRequest) {
        Invoice invoice = invoiceConverter.toInvoice(invoiceCreationRequest);

        // iterate through invoiceCreationRequest.getProducts
        BigDecimal totalAmount = BigDecimal.ZERO;
        BigDecimal realAmount = BigDecimal.ZERO;
        List<InvoiceDetail> invoiceDetails = new ArrayList<>();
        for (Map.Entry<Long, Short> entry : invoiceCreationRequest.getProductQuantity().entrySet()) {
            Long productId = entry.getKey();
            Short quantity = entry.getValue();

            Product product = productRepository.findById(productId)
                    .orElseThrow(() -> new RuntimeException("Product not found"));
            if(product.getInventoryQuantity() < quantity) {
                throw new AppException(ErrorCode.PRODUCT_OUT_OF_STOCK);
            }

            InvoiceDetail invoiceDetail = InvoiceDetail.builder()
                    .invoice(invoice)
                    .product(product)
                    .importPrice(product.getImportPrice())
                    .sellingPrice(product.getSellingPrice())
                    .discountPercent(product.getDiscountPercent())
                    .quantity(quantity)
                    .build();

            invoiceDetails.add(invoiceDetail);
            BigDecimal curAmount = product.getSellingPrice().multiply(BigDecimal.valueOf(quantity));
            totalAmount = totalAmount.add(curAmount);
            realAmount = realAmount.add(curAmount.multiply(BigDecimal.ONE.subtract(product.getDiscountPercent())));
        }
        if(totalAmount.compareTo(invoiceCreationRequest.getTotalAmount()) != 0 || realAmount.compareTo(invoiceCreationRequest.getRealAmount()) != 0) {
            throw new AppException(ErrorCode.TOTAL_AMOUNT_OR_REAL_AMOUNT_NOT_MATCH);
        }
        invoice = invoiceRepository.save(invoice);
        invoiceDetailRepository.saveAll(invoiceDetails);
        return buildInvoiceGeneralResponse(invoice);

    }

    @Override
    @Transactional
    public void deleteInvoice(Long invoiceId) {
        Invoice invoice = invoiceRepository.findById(invoiceId)
                .orElseThrow(() -> new AppException(ErrorCode.INVOICE_NOT_FOUND));
        invoiceDetailRepository.deleteAllByInvoice(invoice);
        invoiceRepository.delete(invoice);
    }

    @Override
    public void deleteInvoices(Long[] invoiceIds) {
        for(Long invoiceId : invoiceIds) {
            deleteInvoice(invoiceId);
        }
    }

    @Override
    public PageResponse<InvoiceGeneralResponse> getInvoicesInPeriodWithPagination(LocalDate startDate, LocalDate endDate, Integer page, Integer size) {

        Pageable pageable = PageRequest.of(page - 1, size);

        Page<Invoice> invoices;
        if(startDate == null && endDate == null) {
            invoices = invoiceRepository.findAll(pageable);
        }
        else if(startDate == null) {
            invoices = invoiceRepository.findAllByCreatedDateBefore(endDate, pageable);
        }
        else if(endDate == null) {
            invoices = invoiceRepository.findAllByCreatedDateAfter(startDate, pageable);
        }
        else {
            invoices = invoiceRepository.findAllByCreatedDateBetween(startDate, endDate, pageable);
        }

        List<InvoiceGeneralResponse> invoiceGeneralResponses = buildInvoiceGeneralResponses(invoices.getContent());
        return PageResponse.<InvoiceGeneralResponse>builder()
                .page(page)
                .size(size)
                .totalElements(invoices.getTotalElements())
                .totalPages(invoices.getTotalPages())
                .data(invoiceGeneralResponses)
                .build();
    }

    @Override
    public List<InvoiceGeneralResponse> getInvoicesInPeriod(LocalDate startDate, LocalDate endDate) {
        List<Invoice> invoices;
        if (startDate == null && endDate == null) {
            invoices = invoiceRepository.findAll();
        } else if (startDate == null) {
            invoices = invoiceRepository.findAllByCreatedDateBefore(endDate);
        } else if (endDate == null) {
            invoices = invoiceRepository.findAllByCreatedDateAfter(startDate);
        } else {
            invoices = invoiceRepository.findAllByCreatedDateBetween(startDate, endDate);
        }

        return buildInvoiceGeneralResponses(invoices);
    }

    @Override
    public InvoiceAggregationResponse getInvoiceAggregation(LocalDate startDate, LocalDate endDate) {
        List<InvoiceGeneralResponse> invoiceGeneralResponses = getInvoicesInPeriod(startDate, endDate);
        BigDecimal totalAmount = BigDecimal.ZERO;
        BigDecimal totalRealAmount = BigDecimal.ZERO;

        for(InvoiceGeneralResponse invoiceGeneralResponse : invoiceGeneralResponses) {
            totalAmount = totalAmount.add(invoiceGeneralResponse.getTotalAmount());
            totalRealAmount = totalRealAmount.add(invoiceGeneralResponse.getRealAmount());
        }
        return InvoiceAggregationResponse.builder()
                .totalAmount(totalAmount)
                .totalRealAmount(totalRealAmount)
                .totalDiscountAmount(totalAmount.subtract(totalRealAmount))
                .build();
    }

    private InvoiceGeneralResponse buildInvoiceGeneralResponse(Invoice invoice) {
        InvoiceGeneralResponse.InvoiceGeneralResponseBuilder builder = InvoiceGeneralResponse.builder()
                .id(invoice.getId())
                .employee(employeeMapper.toEmployeeResponse(invoice.getEmployee()))
                .totalAmount(invoice.getTotalAmount())
                .realAmount(invoice.getRealAmount())
                .createdDate(invoice.getCreatedDate());

        List<InvoiceDetail> invoiceDetails = invoiceDetailRepository.findAllByInvoice(invoice);
        List<InvoiceDetailResponse> invoiceDetailResponses = buildInvoiceDetailResponses(invoiceDetails);
        builder.invoiceDetails(invoiceDetailResponses);

        return builder.build();
    }

    private List<InvoiceGeneralResponse> buildInvoiceGeneralResponses(List<Invoice> invoices) {
        List<InvoiceGeneralResponse> invoiceGeneralResponses = new ArrayList<>();
        for(Invoice invoice : invoices) {
            invoiceGeneralResponses.add(buildInvoiceGeneralResponse(invoice));
        }
        return invoiceGeneralResponses;
    }


    private InvoiceDetailResponse buildInvoiceDetailResponse(InvoiceDetail invoiceDetail) {
        return InvoiceDetailResponse.builder()
                .product(productService.buildProductResponse(invoiceDetail.getProduct()))
                .importPrice(invoiceDetail.getImportPrice())
                .sellingPrice(invoiceDetail.getSellingPrice())
                .discountPercent(invoiceDetail.getDiscountPercent())
                .quantity(invoiceDetail.getQuantity())
                .build();
    }

    private List<InvoiceDetailResponse> buildInvoiceDetailResponses(List<InvoiceDetail> invoiceDetails) {
        List<InvoiceDetailResponse> invoiceDetailResponses = new ArrayList<>();
        for(InvoiceDetail invoiceDetail : invoiceDetails) {
            invoiceDetailResponses.add(buildInvoiceDetailResponse(invoiceDetail));
        }
        return invoiceDetailResponses;
    }
}
