package com.windowprogramming.ClothingStoreManager.repository.custom.product;

import com.windowprogramming.ClothingStoreManager.dto.request.product.ProductSearchRequest;
import com.windowprogramming.ClothingStoreManager.entity.Product;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;

public interface CustomProductRepository {
    Page<Product> searchProducts(ProductSearchRequest request, Pageable pageable);
}
