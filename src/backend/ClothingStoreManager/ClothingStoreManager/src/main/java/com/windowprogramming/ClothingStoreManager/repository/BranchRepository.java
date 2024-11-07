package com.windowprogramming.ClothingStoreManager.repository;

import com.windowprogramming.ClothingStoreManager.entity.Branch;
import com.windowprogramming.ClothingStoreManager.entity.Category;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface BranchRepository extends JpaRepository<Branch, String> {
    List<Branch> findAllByBusinessStatus(boolean b);
}