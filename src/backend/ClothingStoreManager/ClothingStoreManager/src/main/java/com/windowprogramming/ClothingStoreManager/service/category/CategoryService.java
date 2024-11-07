package com.windowprogramming.ClothingStoreManager.service.category;

import com.windowprogramming.ClothingStoreManager.dto.request.category.CategoryCreationRequest;
import com.windowprogramming.ClothingStoreManager.dto.request.category.CategoryUpdateRequest;
import com.windowprogramming.ClothingStoreManager.dto.response.CategoryResponse;

import java.util.List;

public interface CategoryService {
    public CategoryResponse getCategoryById(String categoryId) ;

    public List<CategoryResponse> getAllCategories() ;

    public CategoryResponse createCategory(CategoryCreationRequest categoryCreationRequest);

    public CategoryResponse updateCategory(CategoryUpdateRequest categoryUpdateRequest);

    public void setBusinessStatusToActive(String categoryId);
    public void setBusinessStatusToInactive(String categoryId);
}
