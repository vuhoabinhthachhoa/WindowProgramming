package com.windowprogramming.ClothingStoreManager.mapper;

import com.windowprogramming.ClothingStoreManager.dto.request.category.CategoryCreationRequest;
import com.windowprogramming.ClothingStoreManager.dto.request.category.CategoryUpdateRequest;
import com.windowprogramming.ClothingStoreManager.dto.response.CategoryResponse;
import com.windowprogramming.ClothingStoreManager.entity.Category;
import javax.annotation.processing.Generated;
import org.springframework.stereotype.Component;

@Generated(
    value = "org.mapstruct.ap.MappingProcessor",
    comments = "version: 1.5.5.Final, compiler: javac, environment: Java 21.0.2 (Oracle Corporation)"
)
@Component
public class CategoryMapperImpl implements CategoryMapper {

    @Override
    public Category toCategory(CategoryCreationRequest categoryCreationRequest) {
        if ( categoryCreationRequest == null ) {
            return null;
        }

        Category.CategoryBuilder category = Category.builder();

        category.id( categoryCreationRequest.getId() );
        category.name( categoryCreationRequest.getName() );

        return category.build();
    }

    @Override
    public CategoryResponse toCategoryResponse(Category category) {
        if ( category == null ) {
            return null;
        }

        CategoryResponse.CategoryResponseBuilder categoryResponse = CategoryResponse.builder();

        categoryResponse.id( category.getId() );
        categoryResponse.name( category.getName() );
        categoryResponse.businessStatus( category.getBusinessStatus() );

        return categoryResponse.build();
    }

    @Override
    public void updateCategory(Category category, CategoryUpdateRequest categoryUpdateRequest) {
        if ( categoryUpdateRequest == null ) {
            return;
        }

        if ( categoryUpdateRequest.getId() != null ) {
            category.setId( categoryUpdateRequest.getId() );
        }
        if ( categoryUpdateRequest.getName() != null ) {
            category.setName( categoryUpdateRequest.getName() );
        }
    }
}
