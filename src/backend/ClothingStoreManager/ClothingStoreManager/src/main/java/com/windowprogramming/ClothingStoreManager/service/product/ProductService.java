package com.windowprogramming.ClothingStoreManager.service.product;

import com.windowprogramming.ClothingStoreManager.dto.request.product.ProductCreationRequest;
import com.windowprogramming.ClothingStoreManager.dto.request.product.ProductSearchRequest;
import com.windowprogramming.ClothingStoreManager.dto.request.product.ProductUpdateRequest;
import com.windowprogramming.ClothingStoreManager.dto.response.PageResponse;
import com.windowprogramming.ClothingStoreManager.dto.response.ProductResponse;
import com.windowprogramming.ClothingStoreManager.entity.Product;
import com.windowprogramming.ClothingStoreManager.enums.SortType;
import org.springframework.web.multipart.MultipartFile;

import java.math.BigDecimal;
import java.util.List;

public interface ProductService {
    PageResponse<ProductResponse> searchProducts(ProductSearchRequest productSearchRequest, Integer page, Integer size, String sortField, SortType sortType);
    ProductResponse createProduct(ProductCreationRequest productCreationRequest, MultipartFile image);
    ProductResponse updateProduct(ProductUpdateRequest productUpdateRequest, MultipartFile image);
    void setBusinessStatusToActive(Long productId);
    void setBusinessStatusToActive(List<Long> productIds);
    void setBusinessStatusToInactive(Long productId);
    void setBusinessStatusToInactive(List<Long> productIds);
    void setDiscountPercent(String productCode, BigDecimal discountPercent);
    void setDiscountPercent(List<String> productCodes, BigDecimal discountPercent);

    ProductResponse buildProductResponse(Product product);

}
