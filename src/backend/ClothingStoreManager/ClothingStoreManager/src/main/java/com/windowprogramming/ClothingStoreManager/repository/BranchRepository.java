package com.windowprogramming.ClothingStoreManager.repository;

import com.windowprogramming.ClothingStoreManager.entity.Branch;
import com.windowprogramming.ClothingStoreManager.entity.Category;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

@Repository
public interface BranchRepository extends JpaRepository<Branch, String> {
    List<Branch> findAllByBusinessStatus(boolean b);

    boolean existsByName(String name);

    Optional<Branch> findByName(String name);
}
