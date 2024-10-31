package com.windowprogramming.ClothingStoreManager.mapper;

import com.windowprogramming.ClothingStoreManager.dto.request.branch.BranchCreationRequest;
import com.windowprogramming.ClothingStoreManager.dto.response.BranchResponse;
import com.windowprogramming.ClothingStoreManager.entity.Branch;
import org.mapstruct.Mapper;
import org.mapstruct.NullValuePropertyMappingStrategy;

@Mapper(componentModel = "spring", nullValuePropertyMappingStrategy = NullValuePropertyMappingStrategy.IGNORE)
public interface BranchMapper {
    Branch toBranch(BranchCreationRequest branchCreationRequest);
    BranchResponse toBranchResponse(Branch branch);
    void updateBranch(Branch branch, BranchCreationRequest branchCreationRequest);
}
