package com.windowprogramming.ClothingStoreManager.dto.response.invoice;

import com.fasterxml.jackson.annotation.JsonInclude;
import com.windowprogramming.ClothingStoreManager.dto.response.EmployeeResponse;
import lombok.AccessLevel;
import lombok.Builder;
import lombok.Getter;
import lombok.Setter;
import lombok.experimental.FieldDefaults;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

@Getter
@Setter
@Builder
@FieldDefaults(level = AccessLevel.PRIVATE)
@JsonInclude(JsonInclude.Include.NON_NULL)
public class InvoiceGeneralResponse {
    Long id;
    EmployeeResponse employee;
    LocalDate createdDate;
    BigDecimal totalAmount;
    BigDecimal realAmount;
    List<InvoiceDetailResponse> invoiceDetails;
}
