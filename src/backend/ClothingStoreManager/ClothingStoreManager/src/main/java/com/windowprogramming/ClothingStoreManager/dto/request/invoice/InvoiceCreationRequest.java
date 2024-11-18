package com.windowprogramming.ClothingStoreManager.dto.request.invoice;

import jakarta.validation.constraints.NotEmpty;
import jakarta.validation.constraints.NotNull;
import lombok.AccessLevel;
import lombok.Builder;
import lombok.Getter;
import lombok.Setter;
import lombok.experimental.FieldDefaults;
import org.springframework.http.MediaType;

import java.math.BigDecimal;
import java.util.Map;

@Getter
@Setter
@Builder
@FieldDefaults(level = AccessLevel.PRIVATE)
public class InvoiceCreationRequest {
    @NotNull(message = "REQUIRED_EMPLOYEE_ID")
    Long employeeId;

    @NotNull(message = "REQUIRED_TOTAL_AMOUNT")
    BigDecimal totalAmount;

    @NotNull(message = "REQUIRED_REAL_AMOUNT")
    BigDecimal realAmount;

    @NotEmpty(message = "REQUIRED_PRODUCT_QUANTITY")
    Map<Long, Short> productQuantity;
}
