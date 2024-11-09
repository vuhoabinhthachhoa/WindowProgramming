package com.windowprogramming.ClothingStoreManager.mapper;

import com.windowprogramming.ClothingStoreManager.dto.request.product.ProductCreationRequest;
import com.windowprogramming.ClothingStoreManager.dto.request.product.ProductUpdateRequest;
import com.windowprogramming.ClothingStoreManager.dto.response.ProductResponse;
import com.windowprogramming.ClothingStoreManager.entity.Product;
import javax.annotation.processing.Generated;
import org.springframework.stereotype.Component;

@Generated(
    value = "org.mapstruct.ap.MappingProcessor",
    comments = "version: 1.5.5.Final, compiler: javac, environment: Java 21.0.2 (Oracle Corporation)"
)
@Component
public class ProductMapperImpl implements ProductMapper {

    @Override
    public Product toProduct(ProductCreationRequest productCreationRequest) {
        if ( productCreationRequest == null ) {
            return null;
        }

        Product.ProductBuilder product = Product.builder();

        product.name( productCreationRequest.getName() );
        product.importPrice( productCreationRequest.getImportPrice() );
        product.sellingPrice( productCreationRequest.getSellingPrice() );
        product.inventoryQuantity( productCreationRequest.getInventoryQuantity() );
        product.size( productCreationRequest.getSize() );
        product.discountPercent( productCreationRequest.getDiscountPercent() );

        return product.build();
    }

    @Override
    public ProductResponse toProductResponse(Product product) {
        if ( product == null ) {
            return null;
        }

        ProductResponse.ProductResponseBuilder productResponse = ProductResponse.builder();

        productResponse.id( product.getId() );
        productResponse.code( product.getCode() );
        productResponse.name( product.getName() );
        productResponse.importPrice( product.getImportPrice() );
        productResponse.sellingPrice( product.getSellingPrice() );
        productResponse.inventoryQuantity( product.getInventoryQuantity() );
        productResponse.imageUrl( product.getImageUrl() );
        productResponse.cloudinaryImageId( product.getCloudinaryImageId() );
        productResponse.businessStatus( product.getBusinessStatus() );
        productResponse.size( product.getSize() );
        productResponse.discountPercent( product.getDiscountPercent() );

        return productResponse.build();
    }

    @Override
    public void updateProduct(Product product, ProductUpdateRequest productUpdateRequest) {
        if ( productUpdateRequest == null ) {
            return;
        }

        if ( productUpdateRequest.getId() != null ) {
            product.setId( productUpdateRequest.getId() );
        }
        if ( productUpdateRequest.getImportPrice() != null ) {
            product.setImportPrice( productUpdateRequest.getImportPrice() );
        }
        if ( productUpdateRequest.getSellingPrice() != null ) {
            product.setSellingPrice( productUpdateRequest.getSellingPrice() );
        }
        if ( productUpdateRequest.getInventoryQuantity() != null ) {
            product.setInventoryQuantity( productUpdateRequest.getInventoryQuantity() );
        }
        if ( productUpdateRequest.getDiscountPercent() != null ) {
            product.setDiscountPercent( productUpdateRequest.getDiscountPercent() );
        }
    }
}
