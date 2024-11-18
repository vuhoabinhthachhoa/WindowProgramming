package com.windowprogramming.ClothingStoreManager.service.invoice;

import com.windowprogramming.ClothingStoreManager.dto.request.invoice.InvoiceCreationRequest;
import com.windowprogramming.ClothingStoreManager.dto.response.PageResponse;
import com.windowprogramming.ClothingStoreManager.dto.response.invoice.InvoiceAggregationResponse;
import com.windowprogramming.ClothingStoreManager.dto.response.invoice.InvoiceGeneralResponse;

import java.time.LocalDate;
import java.util.List;

public interface InvoiceService {
    // create invoice
    InvoiceGeneralResponse createInvoice(InvoiceCreationRequest invoiceCreationRequest);
    void deleteInvoice(Long invoiceId);
    void deleteInvoices(Long[] invoiceIds);
    PageResponse<InvoiceGeneralResponse> getInvoicesInPeriodWithPagination(LocalDate startDate, LocalDate endDate, Integer page, Integer size);
    List<InvoiceGeneralResponse> getInvoicesInPeriod(LocalDate startDate, LocalDate endDate);
    InvoiceAggregationResponse getInvoiceAggregation(LocalDate startDate, LocalDate endDate);
}
