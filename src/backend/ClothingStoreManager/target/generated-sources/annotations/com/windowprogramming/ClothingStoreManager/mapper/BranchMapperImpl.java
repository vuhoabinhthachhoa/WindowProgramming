package com.windowprogramming.ClothingStoreManager.mapper;

import com.windowprogramming.ClothingStoreManager.dto.request.branch.BranchCreationRequest;
import com.windowprogramming.ClothingStoreManager.dto.request.branch.BranchUpdateRequest;
import com.windowprogramming.ClothingStoreManager.dto.response.BranchResponse;
import com.windowprogramming.ClothingStoreManager.entity.Branch;
import javax.annotation.processing.Generated;
import org.springframework.stereotype.Component;

@Generated(
    value = "org.mapstruct.ap.MappingProcessor",
    comments = "version: 1.5.5.Final, compiler: javac, environment: Java 21.0.2 (Oracle Corporation)"
)
@Component
public class BranchMapperImpl implements BranchMapper {

    @Override
    public Branch toBranch(BranchCreationRequest branchCreationRequest) {
        if ( branchCreationRequest == null ) {
            return null;
        }

        Branch.BranchBuilder branch = Branch.builder();

        branch.name( branchCreationRequest.getName() );

        return branch.build();
    }

    @Override
    public BranchResponse toBranchResponse(Branch branch) {
        if ( branch == null ) {
            return null;
        }

        BranchResponse.BranchResponseBuilder branchResponse = BranchResponse.builder();

        branchResponse.id( branch.getId() );
        branchResponse.name( branch.getName() );
        branchResponse.businessStatus( branch.getBusinessStatus() );

        return branchResponse.build();
    }

    @Override
    public void updateBranch(Branch branch, BranchUpdateRequest branchUpdateRequest) {
        if ( branchUpdateRequest == null ) {
            return;
        }

        if ( branchUpdateRequest.getName() != null ) {
            branch.setName( branchUpdateRequest.getName() );
        }
    }
}
