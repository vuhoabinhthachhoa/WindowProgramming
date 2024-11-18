package com.windowprogramming.ClothingStoreManager.converter.invoice;

import com.windowprogramming.ClothingStoreManager.dto.request.invoice.InvoiceCreationRequest;
import com.windowprogramming.ClothingStoreManager.dto.response.invoice.InvoiceGeneralResponse;
import com.windowprogramming.ClothingStoreManager.entity.Employee;
import com.windowprogramming.ClothingStoreManager.entity.Invoice;
import com.windowprogramming.ClothingStoreManager.exception.AppException;
import com.windowprogramming.ClothingStoreManager.exception.ErrorCode;
import com.windowprogramming.ClothingStoreManager.repository.EmployeeRepository;
import com.windowprogramming.ClothingStoreManager.repository.ProductRepository;
import jakarta.persistence.Converter;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.stereotype.Component;

import java.time.LocalDate;

@Component
@RequiredArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
public class InvoiceConverter {
    EmployeeRepository employeeRepository;
    ProductRepository productRepository;
    public Invoice toInvoice(InvoiceCreationRequest invoiceCreationRequest) {
        Invoice invoice = new Invoice();
        Employee employee = employeeRepository.findById(invoiceCreationRequest.getEmployeeId())
                .orElseThrow(() -> new AppException(ErrorCode.EMPLOYEE_NOT_FOUND));
        invoice.setEmployee(employee);
        invoice.setTotalAmount(invoiceCreationRequest.getTotalAmount());
        invoice.setRealAmount(invoiceCreationRequest.getRealAmount());
        invoice.setCreatedDate(LocalDate.now());
        return invoice;
    }
}
