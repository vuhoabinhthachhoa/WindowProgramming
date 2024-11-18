package com.windowprogramming.ClothingStoreManager.controller;

import com.windowprogramming.ClothingStoreManager.dto.request.invoice.InvoiceCreationRequest;
import com.windowprogramming.ClothingStoreManager.dto.response.ApiResponse;
import com.windowprogramming.ClothingStoreManager.dto.response.PageResponse;
import com.windowprogramming.ClothingStoreManager.dto.response.invoice.InvoiceAggregationResponse;
import com.windowprogramming.ClothingStoreManager.dto.response.invoice.InvoiceGeneralResponse;
import com.windowprogramming.ClothingStoreManager.enums.SortType;
import com.windowprogramming.ClothingStoreManager.service.invoice.InvoiceService;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.tags.Tag;
import jakarta.validation.Valid;
import jakarta.validation.constraints.NotNull;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import lombok.experimental.NonFinal;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.security.config.annotation.method.configuration.EnableMethodSecurity;
import org.springframework.web.bind.annotation.*;

import java.time.LocalDate;
import java.util.List;

@EnableMethodSecurity
@RestController
@RequestMapping("/invoice")
@RequiredArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
@Tag(name = "Invoice Controller", description = "APIs for managing invoices")
public class InvoiceController {
    InvoiceService invoiceService;

    @NonFinal
    @Value("${app.controller.invoice.response.delete.success}")
    String DELETE_SUCCESS;

    @Operation(summary = "Create invoice", description = "Create a new invoice")
    @PostMapping
    public ApiResponse<InvoiceGeneralResponse> createInvoice(@Valid @RequestBody InvoiceCreationRequest invoiceCreationRequest) {
        return ApiResponse.<InvoiceGeneralResponse>builder()
                .data(invoiceService.createInvoice(invoiceCreationRequest))
                .build();
    }

    @Operation(summary = "Delete invoice", description = "Delete an invoice by ID")
    @DeleteMapping("/{invoiceId}")
    @PreAuthorize("hasAuthority('ADMIN')")
    public ApiResponse<String> deleteInvoice(@PathVariable @NotNull Long invoiceId) {
        invoiceService.deleteInvoice(invoiceId);
        return ApiResponse.<String>builder()
                .data(DELETE_SUCCESS)
                .build();
    }

    @Operation(summary = "Delete invoices", description = "Delete multiple invoices by IDs")
    @DeleteMapping
    @PreAuthorize("hasAuthority('ADMIN')")
    public ApiResponse<String> deleteInvoices(@RequestBody Long[] invoiceIds) {
        invoiceService.deleteInvoices(invoiceIds);
        return ApiResponse.<String>builder()
                .data(DELETE_SUCCESS)
                .build();
    }

    @Operation(summary = "Get invoices with pagination", description = "Retrieve invoices within a date range with pagination and sorting")
    @GetMapping
    @PreAuthorize("hasAuthority('ADMIN')")
    public ApiResponse<PageResponse<InvoiceGeneralResponse>> getInvoicesInPeriodWithPagination(
            @RequestParam(required = false) LocalDate startDate,
            @RequestParam(required = false) LocalDate endDate,
            @RequestParam @NotNull Integer page,
            @RequestParam @NotNull Integer size) {
        return ApiResponse.<PageResponse<InvoiceGeneralResponse>>builder()
                .data(invoiceService.getInvoicesInPeriodWithPagination(startDate, endDate, page, size))
                .build();
    }

//    @Operation(summary = "Get all invoices", description = "Retrieve all invoices within a date range")
//    @GetMapping("/all")
//    @PreAuthorize("hasAuthority('ADMIN')")
//    public ApiResponse<List<InvoiceGeneralResponse>> getInvoicesInPeriod(
//            @RequestParam LocalDate startDate,
//            @RequestParam LocalDate endDate) {
//        return ApiResponse.<List<InvoiceGeneralResponse>>builder()
//                .data(invoiceService.getInvoicesInPeriod(startDate, endDate))
//                .build();
//    }

    @Operation(summary = "Get invoice aggregation", description = "Retrieve aggregated invoice data within a date range")
    @GetMapping("/aggregation")
    @PreAuthorize("hasAuthority('ADMIN')")
    public ApiResponse<InvoiceAggregationResponse> getInvoiceAggregation(
            @RequestParam(required = false) LocalDate startDate,
            @RequestParam(required = false) LocalDate endDate) {
        return ApiResponse.<InvoiceAggregationResponse>builder()
                .data(invoiceService.getInvoiceAggregation(startDate, endDate))
                .build();
    }
}

