package com.windowprogramming.ClothingStoreManager.service.branch;

import com.windowprogramming.ClothingStoreManager.dto.request.branch.BranchCreationRequest;
import com.windowprogramming.ClothingStoreManager.dto.request.branch.BranchUpdateRequest;
import com.windowprogramming.ClothingStoreManager.dto.response.BranchResponse;

import java.util.List;

public interface BranchService {
    List<BranchResponse> getAllBranches();
    BranchResponse getBranchByName(String branchId);
    BranchResponse createBranch(BranchCreationRequest branchCreationRequest);
    BranchResponse updateBranch(BranchUpdateRequest branchUpdateRequest);
    void deleteBranch(String branchId);
}
