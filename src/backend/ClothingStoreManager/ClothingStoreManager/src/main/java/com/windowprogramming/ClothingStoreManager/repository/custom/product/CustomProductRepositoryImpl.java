package com.windowprogramming.ClothingStoreManager.repository.custom.product;

import com.windowprogramming.ClothingStoreManager.dto.request.product.ProductSearchRequest;
import com.windowprogramming.ClothingStoreManager.entity.Product;
import com.windowprogramming.ClothingStoreManager.utils.IsExistingParamUtils;
import jakarta.persistence.EntityManager;
import jakarta.persistence.TypedQuery;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageImpl;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Repository;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

@Repository
@RequiredArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
public class CustomProductRepositoryImpl implements CustomProductRepository {

    EntityManager entityManager;

    @Override
    public Page<Product> searchProducts(ProductSearchRequest request, Pageable pageable) {
        StringBuilder jpql = new StringBuilder("SELECT p FROM products p WHERE 1=1");
        Map<String, Object> parameters = new HashMap<>();

        if(IsExistingParamUtils.isExistingParam(request.getCode())) {
            jpql.append(" AND p.code LIKE :code");
            parameters.put("code", "%" + request.getCode() + "%");
        }

        if (IsExistingParamUtils.isExistingParam(request.getName())) {
            jpql.append(" AND p.name LIKE :name");
            parameters.put("name", "%" + request.getName() + "%");
        }

        if (IsExistingParamUtils.isExistingParam(request.getCategoryName())) {
            jpql.append(" AND p.category.name = :categoryName");
            parameters.put("categoryName", "%" + request.getCategoryName() + "%");
        }

        if (IsExistingParamUtils.isExistingParam(request.getBranchName())) {
            jpql.append(" AND p.branch.name = :branchName");
            parameters.put("branchName", "%" + request.getBranchName() + "%");
        }

//        BigDecimal sellingPriceFrom;
        if(IsExistingParamUtils.isExistingParam(request.getSellingPriceFrom())) {
            jpql.append(" AND p.sellingPrice >= :sellingPriceFrom");
            parameters.put("sellingPriceFrom", request.getSellingPriceFrom());
        }

//        BigDecimal sellingPriceTo;
        if(IsExistingParamUtils.isExistingParam(request.getSellingPriceTo())) {
            jpql.append(" AND p.sellingPrice <= :sellingPriceTo");
            parameters.put("sellingPriceTo", request.getSellingPriceTo());
        }
//        BigDecimal importPriceFrom;
        if(IsExistingParamUtils.isExistingParam(request.getImportPriceFrom())) {
            jpql.append(" AND p.importPrice >= :importPriceFrom");
            parameters.put("importPriceFrom", request.getImportPriceFrom());
        }
//        BigDecimal importPriceTo;
        if(IsExistingParamUtils.isExistingParam(request.getImportPriceTo())) {
            jpql.append(" AND p.importPrice <= :importPriceTo");
            parameters.put("importPriceTo", request.getImportPriceTo());
        }
//        Short inventoryQuantityFrom;
        if(IsExistingParamUtils.isExistingParam(request.getInventoryQuantityFrom())) {
            jpql.append(" AND p.inventoryQuantity >= :inventoryQuantityFrom");
            parameters.put("inventoryQuantityFrom", request.getInventoryQuantityFrom());
        }
//        Short inventoryQuantityTo;
        if(IsExistingParamUtils.isExistingParam(request.getInventoryQuantityTo())) {
            jpql.append(" AND p.inventoryQuantity <= :inventoryQuantityTo");
            parameters.put("inventoryQuantityTo", request.getInventoryQuantityTo());
        }
//        BigDecimal discountPercentFrom;
        if(IsExistingParamUtils.isExistingParam(request.getDiscountPercentFrom())) {
            jpql.append(" AND p.discountPercent >= :discountPercentFrom");
            parameters.put("discountPercentFrom", request.getDiscountPercentFrom());
        }
//        BigDecimal discountPercentTo;
        if(IsExistingParamUtils.isExistingParam(request.getDiscountPercentTo())) {
            jpql.append(" AND p.discountPercent <= :discountPercentTo");
            parameters.put("discountPercentTo", request.getDiscountPercentTo());
        }
//        Boolean businessStatus;
        if(IsExistingParamUtils.isExistingParam(request.getBusinessStatus())) {
            jpql.append(" AND p.businessStatus = :businessStatus");
            parameters.put("businessStatus", request.getBusinessStatus());
        }


        // Apply sorting
        if (pageable.getSort().isSorted()) {
            jpql.append(" ORDER BY ");
            for (Sort.Order order : pageable.getSort()) {
                jpql.append("p.").append(order.getProperty()).append(" ").append(order.getDirection()).append(", ");
            }
            jpql.setLength(jpql.length() - 2); // Remove the trailing comma and space
        }

        TypedQuery<Product> query = entityManager.createQuery(jpql.toString(), Product.class);
        parameters.forEach(query::setParameter);

        query.setFirstResult((int) pageable.getOffset());
        query.setMaxResults(pageable.getPageSize());

        List<Product> products = query.getResultList();

        TypedQuery<Long> countQuery = entityManager.createQuery(
                "SELECT COUNT(p) FROM products p WHERE 1=1", Long.class);
        Long count = countQuery.getSingleResult();

        return new PageImpl<>(products, pageable, count);

    }
}
