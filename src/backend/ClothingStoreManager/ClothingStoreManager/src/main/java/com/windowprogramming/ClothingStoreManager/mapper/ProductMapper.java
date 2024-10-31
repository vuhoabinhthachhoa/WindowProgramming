package com.windowprogramming.ClothingStoreManager.mapper;

import com.windowprogramming.ClothingStoreManager.dto.request.product.ProductCreationRequest;
import com.windowprogramming.ClothingStoreManager.dto.response.ProductResponse;
import com.windowprogramming.ClothingStoreManager.entity.Product;
import org.mapstruct.Mapper;
import org.mapstruct.Mapping;
import org.mapstruct.NullValuePropertyMappingStrategy;

@Mapper(componentModel = "spring", nullValuePropertyMappingStrategy = NullValuePropertyMappingStrategy.IGNORE)
public interface ProductMapper {

    @Mapping(target = "branch", ignore = true)
    @Mapping(target = "category", ignore = true)
    Product toProduct(ProductCreationRequest productCreationRequest);

    @Mapping(target = "branch", ignore = true)
    @Mapping(target = "category", ignore = true)
    ProductResponse toProductResponse(Product product);

    @Mapping(target = "branch", ignore = true)
    @Mapping(target = "category", ignore = true)
    void updateProduct(Product product, ProductCreationRequest productCreationRequest);
}
