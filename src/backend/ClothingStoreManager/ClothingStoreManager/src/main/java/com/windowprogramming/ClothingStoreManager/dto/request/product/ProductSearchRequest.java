package com.windowprogramming.ClothingStoreManager.dto.request.product;

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
public class ProductSearchRequest {
    String code;
    String name;
    String categoryName;
    String branchName;
    BigDecimal sellingPriceFrom;
    BigDecimal sellingPriceTo;
    BigDecimal importPriceFrom;
    BigDecimal importPriceTo;
    Short inventoryQuantityFrom;
    Short inventoryQuantityTo;
    BigDecimal discountPercentFrom;
    BigDecimal discountPercentTo;
    Boolean businessStatus;

}
