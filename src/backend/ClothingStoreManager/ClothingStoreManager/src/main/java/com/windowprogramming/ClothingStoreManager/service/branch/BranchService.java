package com.windowprogramming.ClothingStoreManager.service.branch;

import com.windowprogramming.ClothingStoreManager.dto.request.branch.BranchCreationRequest;
import com.windowprogramming.ClothingStoreManager.dto.response.BranchResponse;

import java.util.List;

public interface BranchService {
    List<BranchResponse> getAllBranches();
    BranchResponse getBranchById(Long branchId);
    BranchResponse createBranch(BranchCreationRequest branchCreationRequest);
    BranchResponse updateBranch(Long id, BranchCreationRequest branchCreationRequest);
    void deleteBranch(Long branchId);
}
