package com.windowprogramming.ClothingStoreManager.service.category;

import com.windowprogramming.ClothingStoreManager.dto.request.category.CategoryCreationRequest;
import com.windowprogramming.ClothingStoreManager.dto.request.category.CategoryUpdateRequest;
import com.windowprogramming.ClothingStoreManager.dto.response.CategoryResponse;
import com.windowprogramming.ClothingStoreManager.entity.Category;
import com.windowprogramming.ClothingStoreManager.exception.AppException;
import com.windowprogramming.ClothingStoreManager.exception.ErrorCode;
import com.windowprogramming.ClothingStoreManager.mapper.CategoryMapper;
import com.windowprogramming.ClothingStoreManager.repository.CategoryRepository;
import com.windowprogramming.ClothingStoreManager.repository.ProductRepository;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;

@Service
@RequiredArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
public class CategoryServiceImpl implements CategoryService {
    CategoryRepository categoryRepository;
    ProductRepository productRepository;

    CategoryMapper categoryMapper;

    @Override
    public void deleteCategory(String categoryId) {
        Category category = categoryRepository.findById(categoryId)
                .orElseThrow(() -> new AppException(ErrorCode.CATEGORY_NOT_FOUND));
        productRepository.deleteAllByCategory(category);
        categoryRepository.delete(category);
    }

    @Override
    public CategoryResponse getCategoryById(String categoryId) {
        Category category = categoryRepository.findById(categoryId)
                .orElseThrow(() -> new AppException(ErrorCode.CATEGORY_NOT_FOUND));
        return categoryMapper.toCategoryResponse(category);
    }

    @Override
    public List<CategoryResponse> getAllCategories() {
        List<Category> categories = categoryRepository.findAll();

        List<CategoryResponse> categoryResponses = new ArrayList<>();
        for(Category category : categories) {
            categoryResponses.add(categoryMapper.toCategoryResponse(category));
        }
        return categoryResponses;
    }

    @Override
    public CategoryResponse createCategory(CategoryCreationRequest categoryCreationRequest) {
        if(categoryRepository.existsById(categoryCreationRequest.getId()) || categoryRepository.existsByName(categoryCreationRequest.getName())) {
            throw new AppException(ErrorCode.CATEGORY_EXISTED);
        }

        Category category = categoryMapper.toCategory(categoryCreationRequest);
        category = categoryRepository.save(category);
        return categoryMapper.toCategoryResponse(category);
    }

    @Override
    public CategoryResponse updateCategory(CategoryUpdateRequest categoryUpdateRequest) {

        Category category = categoryRepository.findById(categoryUpdateRequest.getId())
                .orElseThrow(() -> new AppException(ErrorCode.CATEGORY_NOT_FOUND));
        categoryMapper.updateCategory(category, categoryUpdateRequest);
        category = categoryRepository.save(category);
        return categoryMapper.toCategoryResponse(category);

    }
}
