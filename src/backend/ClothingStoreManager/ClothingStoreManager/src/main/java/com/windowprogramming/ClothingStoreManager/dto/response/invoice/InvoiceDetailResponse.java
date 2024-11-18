package com.windowprogramming.ClothingStoreManager.dto.response.invoice;

import com.fasterxml.jackson.annotation.JsonInclude;
import com.windowprogramming.ClothingStoreManager.dto.response.ProductResponse;
import lombok.AccessLevel;
import lombok.Builder;
import lombok.Getter;
import lombok.Setter;
import lombok.experimental.FieldDefaults;

import java.math.BigDecimal;

@Getter
@Setter
@Builder
@FieldDefaults(level = AccessLevel.PRIVATE)
@JsonInclude(JsonInclude.Include.NON_NULL)
public class InvoiceDetailResponse {
    //Long id;
    ProductResponse product;
    BigDecimal importPrice;
    BigDecimal sellingPrice;
    BigDecimal discountPercent;
    Short quantity;
}