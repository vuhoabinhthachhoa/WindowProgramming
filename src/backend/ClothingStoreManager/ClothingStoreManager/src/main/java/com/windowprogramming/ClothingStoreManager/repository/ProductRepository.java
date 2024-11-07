package com.windowprogramming.ClothingStoreManager.repository;

import com.windowprogramming.ClothingStoreManager.entity.Branch;
import com.windowprogramming.ClothingStoreManager.entity.Category;
import com.windowprogramming.ClothingStoreManager.entity.Product;
import com.windowprogramming.ClothingStoreManager.enums.Size;
import com.windowprogramming.ClothingStoreManager.repository.custom.product.CustomProductRepository;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

@Repository
public interface ProductRepository extends JpaRepository<Product, Long>, CustomProductRepository {

    void deleteAllByCategory(Category category);

    void deleteAllByBranch(Branch branch);

    boolean existsByCodeAndSize(String code, Size size);


    Optional<Product> findByCode(String productCode);

    List<Product> findAllByCategory(Category category);

    List<Product> findAllByBranch(Branch branch);

    Product findByNameAndCategoryAndBranchAndSize(String name, Category category, Branch branch, Size size);

    List<Product> findAllByNameAndCategoryAndBranch(String name, Category category, Branch branch);

    List<Product> findAllByCode(String productCode);
}
