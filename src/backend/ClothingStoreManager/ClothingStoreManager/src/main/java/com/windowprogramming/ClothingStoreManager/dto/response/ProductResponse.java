package com.windowprogramming.ClothingStoreManager.dto.response;

import com.fasterxml.jackson.annotation.JsonInclude;
import com.windowprogramming.ClothingStoreManager.enums.Size;
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
public class ProductResponse {
    Long id;
    String code;
    String name;
    CategoryResponse category;
    BigDecimal importPrice;
    BigDecimal sellingPrice;
    BranchResponse branch;
    Short inventoryQuantity;
    String imageUrl;
    String cloudinaryImageId;
    Boolean businessStatus;
    Size size;
    BigDecimal discountPercent;
}
