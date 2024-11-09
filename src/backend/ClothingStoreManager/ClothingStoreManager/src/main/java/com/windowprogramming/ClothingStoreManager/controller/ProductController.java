package com.windowprogramming.ClothingStoreManager.controller;

import com.windowprogramming.ClothingStoreManager.dto.request.product.ProductCreationRequest;
import com.windowprogramming.ClothingStoreManager.dto.request.product.ProductSearchRequest;
import com.windowprogramming.ClothingStoreManager.dto.request.product.ProductUpdateRequest;
import com.windowprogramming.ClothingStoreManager.dto.response.ApiResponse;
import com.windowprogramming.ClothingStoreManager.dto.response.PageResponse;
import com.windowprogramming.ClothingStoreManager.dto.response.ProductResponse;
import com.windowprogramming.ClothingStoreManager.enums.SortType;
import com.windowprogramming.ClothingStoreManager.service.product.ProductService;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.tags.Tag;
import jakarta.validation.Valid;
import jakarta.validation.constraints.NotNull;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import lombok.experimental.NonFinal;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.MediaType;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.security.config.annotation.method.configuration.EnableMethodSecurity;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;

import java.math.BigDecimal;
import java.util.List;

@EnableMethodSecurity
@RestController
@RequestMapping("/product")
@RequiredArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
@Tag(name = "Product Controller", description = "APIs for managing products")
public class ProductController {

    ProductService productService;

    @NonFinal
    @Value("${app.controller.product.response.stop-business.success}")
    String STOP_BUSINESS_SUCCESS;

    @NonFinal
    @Value("${app.controller.product.response.continue-business.success}")
    String CONTINUE_BUSINESS_SUCCESS;

    @NonFinal
    @Value("${app.controller.product.response.set-discount.success}")
    String SET_DISCOUNT_SUCCESS;

    @PostMapping(consumes = MediaType.MULTIPART_FORM_DATA_VALUE)
    @Operation(summary = "Create product", description = "Create a new product")
    @PreAuthorize("hasAuthority('ADMIN')")
    public ApiResponse<ProductResponse> createProduct(@Valid @RequestPart("data") ProductCreationRequest productCreationRequest, @RequestPart("file") MultipartFile image) {
        return ApiResponse.<ProductResponse>builder()
                .data(productService.createProduct(productCreationRequest, image))
                .build();
    }

    @PutMapping()
    @Operation(summary = "Update product", description = "Update an existing product")
    @PreAuthorize("hasAuthority('ADMIN')")
    public ApiResponse<ProductResponse> updateProduct(@Valid @RequestPart("data") ProductUpdateRequest productUpdateRequest, @RequestPart("file") MultipartFile image) {
        return ApiResponse.<ProductResponse>builder()
                .data(productService.updateProduct(productUpdateRequest, image))
                .build();
    }

    @GetMapping("/search")
    @Operation(summary = "Search products", description = "Search for products with pagination and sorting")
    @PreAuthorize("hasAuthority('ADMIN')")
    public ApiResponse<PageResponse<ProductResponse>> searchProducts(@Valid @RequestBody ProductSearchRequest productSearchRequest,
                                                                     @RequestParam @NotNull Integer page,
                                                                     @RequestParam @NotNull Integer size,
                                                                     @RequestParam @NotNull String sortField,
                                                                     @RequestParam @NotNull SortType sortType) {
        return ApiResponse.<PageResponse<ProductResponse>>builder()
                .data(productService.searchProducts(productSearchRequest, page, size, sortField, sortType))
                .build();
    }

    @PatchMapping("/status/active")
    @Operation(summary = "Set product status to active", description = "Set the business status of a product to active")
    @PreAuthorize("hasAuthority('ADMIN')")
    public ApiResponse<String> setBusinessStatusToActive(@RequestParam @NotNull Long productId) {
        productService.setBusinessStatusToActive(productId);
        return ApiResponse.<String>builder()
                .data(CONTINUE_BUSINESS_SUCCESS)
                .build();
    }

    @PatchMapping("/status/active/bulk")
    @Operation(summary = "Set products status to active", description = "Set the business status of multiple products to active")
    @PreAuthorize("hasAuthority('ADMIN')")
    public ApiResponse<String> setBusinessStatusesToActive(@RequestBody List<Long> productIds) {
        productService.setBusinessStatusToActive(productIds);
        return ApiResponse.<String>builder()
                .data(CONTINUE_BUSINESS_SUCCESS)
                .build();
    }

    @PatchMapping("/status/inactive")
    @Operation(summary = "Set product status to inactive", description = "Set the business status of a product to inactive")
    @PreAuthorize("hasAuthority('ADMIN')")
    public ApiResponse<String> setBusinessStatusToInactive(@RequestParam @NotNull Long productId) {
        productService.setBusinessStatusToInactive(productId);
        return ApiResponse.<String>builder()
                .data(STOP_BUSINESS_SUCCESS)
                .build();
    }

    @PatchMapping("/status/inactive/bulk")
    @Operation(summary = "Set products status to inactive", description = "Set the business status of multiple products to inactive")
    @PreAuthorize("hasAuthority('ADMIN')")
    public ApiResponse<String> setBusinessStatusToInactive(@RequestBody List<Long> productIds) {
        productService.setBusinessStatusToInactive(productIds);
        return ApiResponse.<String>builder()
                .data(STOP_BUSINESS_SUCCESS)
                .build();
    }

    @PatchMapping("/discount")
    @Operation(summary = "Set product discount", description = "Set the discount percent of a product")
    @PreAuthorize("hasAuthority('ADMIN')")
    public ApiResponse<String> setDiscountPercent(@RequestParam @NotNull String productCode, @RequestParam @NotNull BigDecimal discountPercent) {
        productService.setDiscountPercent(productCode, discountPercent);
        return ApiResponse.<String>builder()
                .data(SET_DISCOUNT_SUCCESS)
                .build();
    }

    @PatchMapping("/discount/bulk")
    @Operation(summary = "Set products discount", description = "Set the discount percent of multiple products")
    @PreAuthorize("hasAuthority('ADMIN')")
    public ApiResponse<String> setDiscountPercent(@RequestBody List<String> productCodes, @RequestParam @NotNull BigDecimal discountPercent) {
        productService.setDiscountPercent(productCodes, discountPercent);
        return ApiResponse.<String>builder()
                .data(SET_DISCOUNT_SUCCESS)
                .build();
    }
}