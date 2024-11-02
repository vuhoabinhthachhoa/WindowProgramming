package com.windowprogramming.ClothingStoreManager.service.product;

import com.windowprogramming.ClothingStoreManager.dto.request.product.ProductCreationRequest;
import com.windowprogramming.ClothingStoreManager.dto.request.product.ProductSearchRequest;
import com.windowprogramming.ClothingStoreManager.dto.request.product.ProductUpdateRequest;
import com.windowprogramming.ClothingStoreManager.dto.response.CloudinaryResponse;
import com.windowprogramming.ClothingStoreManager.dto.response.PageResponse;
import com.windowprogramming.ClothingStoreManager.dto.response.ProductResponse;
import com.windowprogramming.ClothingStoreManager.entity.Branch;
import com.windowprogramming.ClothingStoreManager.entity.Category;
import com.windowprogramming.ClothingStoreManager.entity.Product;
import com.windowprogramming.ClothingStoreManager.enums.SortType;
import com.windowprogramming.ClothingStoreManager.exception.AppException;
import com.windowprogramming.ClothingStoreManager.exception.ErrorCode;
import com.windowprogramming.ClothingStoreManager.mapper.BranchMapper;
import com.windowprogramming.ClothingStoreManager.mapper.CategoryMapper;
import com.windowprogramming.ClothingStoreManager.mapper.ProductMapper;
import com.windowprogramming.ClothingStoreManager.repository.BranchRepository;
import com.windowprogramming.ClothingStoreManager.repository.CategoryRepository;
import com.windowprogramming.ClothingStoreManager.repository.ProductRepository;
import com.windowprogramming.ClothingStoreManager.service.CloudinaryService;
import com.windowprogramming.ClothingStoreManager.utils.FileUploadUtils;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Service;
import org.springframework.web.multipart.MultipartFile;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

@Service
@RequiredArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
public class ProductServiceImpl implements ProductService {
    ProductRepository productRepository;
    BranchRepository branchRepository;
    CategoryRepository categoryRepository;

    CloudinaryService cloudinaryService;

    ProductMapper productMapper;
    BranchMapper branchMapper;
    CategoryMapper categoryMapper;

    @Override
    public PageResponse<ProductResponse> searchProducts(ProductSearchRequest productSearchRequest, Integer page, Integer size, String sortField, SortType sortType) {
        Pageable pageable = PageRequest.of(page - 1, size, sortType == SortType.ASC ? Sort.by(sortField).ascending() : Sort.by(sortField).descending());
        Page<Product> products = productRepository.searchProducts(productSearchRequest, pageable);
        List<ProductResponse> responses = buildProductResponses(products.getContent());

        return PageResponse.<ProductResponse>builder()
                .page(page)
                .size(size)
                .totalElements(products.getTotalElements())
                .totalPages(products.getTotalPages())
                .data(responses)
                .build();
    }

    @Override
    public ProductResponse createProduct(ProductCreationRequest productCreationRequest, MultipartFile image) {
        if(productRepository.existsByCodeAndSize(productCreationRequest.getCode(), productCreationRequest.getSize())) {
            throw new AppException(ErrorCode.PRODUCT_EXISTED);
        }
        Product product = productMapper.toProduct(productCreationRequest);
        product.setBranch(branchRepository.findById(productCreationRequest.getBranchId())
                .orElseThrow(() -> new AppException(ErrorCode.BRANCH_NOT_FOUND)));

        product.setCategory(categoryRepository.findById(productCreationRequest.getCategoryId())
                .orElseThrow(() -> new AppException(ErrorCode.CATEGORY_NOT_FOUND)));
        product = productRepository.save(product);

        // upload image to cloudinary
        FileUploadUtils.assertAllowed(image, FileUploadUtils.IMAGE_PATTERN);
        final String fileName = FileUploadUtils.getFileName(image.getOriginalFilename());
        final CloudinaryResponse response = this.cloudinaryService.uploadFile(image, fileName);
        product.setImageUrl(response.getUrl());
        product.setCloudinaryImageId(response.getPublicId());

        return buildProductResponse(product);
    }

    @Override
    public ProductResponse updateProduct(ProductUpdateRequest productUpdateRequest, MultipartFile image) {
        Product product = productRepository.findById(productUpdateRequest.getId())
                .orElseThrow(() -> new AppException(ErrorCode.PRODUCT_NOT_FOUND));
        productMapper.updateProduct(product, productUpdateRequest);

        Branch branch = branchRepository.findById(productUpdateRequest.getBranchName())
                .orElseThrow(() -> new AppException(ErrorCode.BRANCH_NOT_FOUND));
        product.setBranch(branch);

        Category category = categoryRepository.findById(productUpdateRequest.getCategoryId())
                .orElseThrow(() -> new AppException(ErrorCode.CATEGORY_NOT_FOUND));
        product.setCategory(category);

        if(image != null) {
            FileUploadUtils.assertAllowed(image, FileUploadUtils.IMAGE_PATTERN);
            final String fileName = FileUploadUtils.getFileName(image.getOriginalFilename());
            final CloudinaryResponse response = this.cloudinaryService.uploadFile(image, fileName);
            product.setImageUrl(response.getUrl());
            product.setCloudinaryImageId(response.getPublicId());
        }

        product = productRepository.save(product);
        return productMapper.toProductResponse(product);
    }

    @Override
    public void deleteProducts(List<String> productCodes) {

    }

    @Override
    public void deleteProduct(String productCode) {

    }

    @Override
    public void setBusinessStatusToActive(String productCode) {
        Product product = productRepository.findByCode(productCode)
                .orElseThrow(() -> new AppException(ErrorCode.PRODUCT_NOT_FOUND));
        product.setBusinessStatus(true);
        productRepository.save(product);
    }

    @Override
    public void setBusinessStatusToActive(List<String> productCodes) {
        for(String productCode : productCodes) {
            setBusinessStatusToActive(productCode);
        }
    }

    @Override
    public void setBusinessStatusToInactive(String productCode) {
        Product product = productRepository.findByCode(productCode)
                .orElseThrow(() -> new AppException(ErrorCode.PRODUCT_NOT_FOUND));
        product.setBusinessStatus(false);
        productRepository.save(product);
    }

    @Override
    public void setBusinessStatusToInactive(List<String> productCodes) {
        for(String productCode : productCodes) {
            setBusinessStatusToInactive(productCode);
        }
    }

    @Override
    public void setDiscountPercent(String productCode, BigDecimal discountPercent) {
        Product product = productRepository.findByCode(productCode)
                .orElseThrow(() -> new AppException(ErrorCode.PRODUCT_NOT_FOUND));
        product.setDiscountPercent(discountPercent);
        productRepository.save(product);
    }


    @Override
    public void setDiscountPercent(List<String> productCodes, BigDecimal discountPercent) {
        for(String productCode : productCodes) {
            setDiscountPercent(productCode, discountPercent);
        }
    }

    private ProductResponse buildProductResponse(Product product) {
        ProductResponse productResponse = productMapper.toProductResponse(product);
        productResponse.setBranch(branchMapper.toBranchResponse(product.getBranch()));
        productResponse.setCategory(categoryMapper.toCategoryResponse(product.getCategory()));
        return productResponse;
    }

    private List<ProductResponse> buildProductResponses(List<Product> products) {
        List<ProductResponse> productResponses = new ArrayList<>();
        for(Product product : products) {
            productResponses.add(buildProductResponse(product));
        }
        return productResponses;
    }
}
