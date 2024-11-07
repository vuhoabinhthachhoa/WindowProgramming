package com.windowprogramming.ClothingStoreManager.dto.request.product;

import com.windowprogramming.ClothingStoreManager.enums.Size;
import jakarta.validation.constraints.DecimalMax;
import jakarta.validation.constraints.DecimalMin;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
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
public class ProductCreationRequest {

    @NotBlank(message = "REQUIRED_PRODUCT_NAME")
    String name;

    @NotBlank(message = "REQUIRED_CATEGORY_ID")
    String categoryId;

    @NotNull(message = "REQUIRED_IMPORT_PRICE")
    BigDecimal importPrice;

    @NotNull(message = "REQUIRED_SELLING_PRICE")
    BigDecimal sellingPrice;

    @NotNull(message = "REQUIRED_BRANCH_NAME")
    String branchName;

    @NotNull(message = "REQUIRED_INVENTORY_QUANTITY")
    Short inventoryQuantity;

    @NotNull(message = "REQUIRED_SIZE")
    Size size;

    @DecimalMin(value = "0.00", message = "VALID_DISCOUNT_PERCENT")
    @DecimalMax(value = "1.00", message = "VALID_DISCOUNT_PERCENT")
    BigDecimal discountPercent;
}