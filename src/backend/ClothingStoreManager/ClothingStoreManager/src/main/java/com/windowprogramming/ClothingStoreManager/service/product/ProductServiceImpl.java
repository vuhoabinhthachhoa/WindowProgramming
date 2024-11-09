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

        Product product = productMapper.toProduct(productCreationRequest);
        product.setBranch(branchRepository.findByName(productCreationRequest.getBranchName())
                .orElseThrow(() -> new AppException(ErrorCode.BRANCH_NOT_FOUND)));

        product.setCategory(categoryRepository.findById(productCreationRequest.getCategoryId())
                .orElseThrow(() -> new AppException(ErrorCode.CATEGORY_NOT_FOUND)));

        product.setId(productRepository.getTheLastProductId() + 1);

        List<Product> existedProduct = productRepository.findAllByNameAndCategoryAndBranch(product.getName(), product.getCategory(), product.getBranch());
        // products with the same name, size, branch, and category have the same code
        if(existedProduct!= null && !existedProduct.isEmpty()) {
            String code = existedProduct.getFirst().getCode();
            // check if product with the same size, branch, category, and name existed
           if(productRepository.existsByCodeAndSize(code, product.getSize())) {
               throw new AppException(ErrorCode.PRODUCT_EXISTED);
           }
           else {
               product.setCode(code);
           }
        }
        else {
            product.setCode(product.getCategory().getId() + product.getId());
        }



        if(productRepository.existsByCodeAndSize(product.getCode(), product.getSize())) {
            throw new AppException(ErrorCode.PRODUCT_EXISTED);
        }

        // upload image to cloudinary
        if(image != null && !image.isEmpty()) {
            FileUploadUtils.assertAllowed(image, FileUploadUtils.IMAGE_PATTERN);
            final String fileName = FileUploadUtils.getFileName(image.getOriginalFilename());
            final CloudinaryResponse response = this.cloudinaryService.uploadFile(image, fileName);
            product.setImageUrl(response.getUrl());
            product.setCloudinaryImageId(response.getPublicId());
        }

        product = productRepository.save(product);

        return buildProductResponse(product);
    }

    @Override
    public ProductResponse updateProduct(ProductUpdateRequest productUpdateRequest, MultipartFile image) {
        Product product = productRepository.findById(productUpdateRequest.getId())
                .orElseThrow(() -> new AppException(ErrorCode.PRODUCT_NOT_FOUND));

        productMapper.updateProduct(product, productUpdateRequest);

        if(image != null && !image.isEmpty()) {
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
    public void setBusinessStatusToActive(Long productId) {
        Product product = productRepository.findById(productId)
                .orElseThrow(() -> new AppException(ErrorCode.PRODUCT_NOT_FOUND));
        product.setBusinessStatus(true);
        productRepository.save(product);
    }

    @Override
    public void setBusinessStatusToActive(List<Long> productIds) {
        for(Long productId : productIds) {
            setBusinessStatusToActive(productId);
        }
    }

    @Override
    public void setBusinessStatusToInactive(Long productId) {
        Product product = productRepository.findById(productId)
                .orElseThrow(() -> new AppException(ErrorCode.PRODUCT_NOT_FOUND));
        product.setBusinessStatus(false);
        productRepository.save(product);
    }

    @Override
    public void setBusinessStatusToInactive(List<Long> productIds) {
        for(Long productId : productIds) {
            setBusinessStatusToInactive(productId);
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

    @Override
    public ProductResponse buildProductResponse(Product product) {
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
