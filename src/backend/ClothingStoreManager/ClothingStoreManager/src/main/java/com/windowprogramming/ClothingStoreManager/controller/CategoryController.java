package com.windowprogramming.ClothingStoreManager.controller;


import com.windowprogramming.ClothingStoreManager.dto.request.category.CategoryCreationRequest;
import com.windowprogramming.ClothingStoreManager.dto.request.category.CategoryUpdateRequest;
import com.windowprogramming.ClothingStoreManager.dto.response.ApiResponse;
import com.windowprogramming.ClothingStoreManager.dto.response.CategoryResponse;
import com.windowprogramming.ClothingStoreManager.service.category.CategoryService;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.tags.Tag;
import jakarta.validation.Valid;
import jakarta.validation.constraints.NotNull;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import lombok.experimental.NonFinal;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.security.config.annotation.method.configuration.EnableMethodSecurity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@EnableMethodSecurity
@RestController
@RequestMapping("/category")
@RequiredArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
@Tag(name = "Category Controller", description = "APIs for managing categories")
public class CategoryController {
    CategoryService categoryService;

    @NonFinal
    @Value("${app.controller.category.response.stop-business.success}")
    String STOP_BUSINESS_SUCCESS;

    @PostMapping()
    @Operation(summary = "Create category",
            description = "Create category")
    @PreAuthorize("hasAuthority('ADMIN')")
    public ApiResponse<CategoryResponse> createCategory(@Valid @RequestBody CategoryCreationRequest categoryCreationRequest) {
        return ApiResponse.<CategoryResponse>builder()
                .data(categoryService.createCategory(categoryCreationRequest))
                .build();
    }

    @GetMapping("/all")
    @Operation(summary = "Get all categories",
            description = "Get all categories which are in business")
    @PreAuthorize("hasAuthority('ADMIN')")
    public ApiResponse<List<CategoryResponse>> getAllCategories() {
        return ApiResponse.<List<CategoryResponse>>builder()
                .data(categoryService.getAllCategories())
                .build();
    }

    @GetMapping()
    @Operation(summary = "Get category by id",
            description = "Get category by id")
    @PreAuthorize("hasAuthority('ADMIN')")
    public ApiResponse<CategoryResponse> getCategoryById(@RequestParam String categoryId) {
        return ApiResponse.<CategoryResponse>builder()
                .data(categoryService.getCategoryById(categoryId))
                .build();
    }


    @PutMapping()
    @Operation(summary = "Update category",
            description = "Update category")
    @PreAuthorize("hasAuthority('ADMIN')")
    public ApiResponse<CategoryResponse> updateCategory(@Valid @RequestBody CategoryUpdateRequest categoryUpdateRequest) {
        return ApiResponse.<CategoryResponse>builder()
                .data(categoryService.updateCategory(categoryUpdateRequest))
                .build();
    }

    @PatchMapping("status/inactive")
    @Operation(summary = "Set the category business status to inactive",
            description = "Set the category business status to inactive")
    @PreAuthorize("hasAuthority('ADMIN')")
    public ApiResponse<String> setBusinessStatusToInactive(@RequestParam @NotNull String categoryId) {
        categoryService.setBusinessStatusToInactive(categoryId);
        return ApiResponse.<String>builder()
                .data(STOP_BUSINESS_SUCCESS)
                .build();
    }
}
