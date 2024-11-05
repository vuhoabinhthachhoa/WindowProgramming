package com.windowprogramming.ClothingStoreManager.service.branch;

import com.windowprogramming.ClothingStoreManager.dto.request.branch.BranchCreationRequest;
import com.windowprogramming.ClothingStoreManager.dto.request.branch.BranchUpdateRequest;
import com.windowprogramming.ClothingStoreManager.dto.response.BranchResponse;
import com.windowprogramming.ClothingStoreManager.entity.Branch;
import com.windowprogramming.ClothingStoreManager.entity.Product;
import com.windowprogramming.ClothingStoreManager.exception.AppException;
import com.windowprogramming.ClothingStoreManager.exception.ErrorCode;
import com.windowprogramming.ClothingStoreManager.mapper.BranchMapper;
import com.windowprogramming.ClothingStoreManager.repository.BranchRepository;
import com.windowprogramming.ClothingStoreManager.repository.ProductRepository;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;

@Service
@RequiredArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
public class BranchServiceImpl implements BranchService {
    BranchRepository branchRepository;
    ProductRepository productRepository;

    BranchMapper branchMapper;
    @Override
    public List<BranchResponse> getAllBranches() {
        List<Branch> branches = branchRepository.findAllByBusinessStatus(true);
        List<BranchResponse> branchResponses = new ArrayList<>();
        for(Branch branch : branches) {
            branchResponses.add(branchMapper.toBranchResponse(branch));
        }
        return branchResponses;
    }

    @Override
    public BranchResponse getBranchByName(String branchId) {
        Branch branch = branchRepository.findById(branchId)
                .orElseThrow(() -> new AppException(ErrorCode.BRAND_NOT_FOUND));
        return branchMapper.toBranchResponse(branch);
    }

    @Override
    public BranchResponse createBranch(BranchCreationRequest branchCreationRequest) {
        if(branchRepository.existsById(branchCreationRequest.getName())) {
            throw new AppException(ErrorCode.BRANCH_EXISTED);
        }
        Branch branch = branchMapper.toBranch(branchCreationRequest);
        branch = branchRepository.save(branch);
        return branchMapper.toBranchResponse(branch);
    }

    @Override
    public BranchResponse updateBranch(BranchUpdateRequest branchUpdateRequest) {
        Branch branch = branchRepository.findById(branchUpdateRequest.getName())
                .orElseThrow(() -> new AppException(ErrorCode.BRAND_NOT_FOUND));
        branchMapper.updateBranch(branch, branchUpdateRequest);
        branch = branchRepository.save(branch);
        return branchMapper.toBranchResponse(branch);
    }

    @Override
    public void setBusinessStatusToActive(String branchId) {
        Branch branch = branchRepository.findById(branchId)
                .orElseThrow(() -> new AppException(ErrorCode.BRAND_NOT_FOUND));
        branch.setBusinessStatus(true);
        List<Product> products = productRepository.findAllByBranch(branch);
        for(Product product : products) {
            product.setBusinessStatus(true);
            productRepository.save(product);
        }
        branchRepository.save(branch);
    }

    @Override
    public void setBusinessStatusToInactive(String branchId) {
        Branch branch = branchRepository.findById(branchId)
                .orElseThrow(() -> new AppException(ErrorCode.BRAND_NOT_FOUND));
        branch.setBusinessStatus(false);
        List<Product> products = productRepository.findAllByBranch(branch);
        for(Product product : products) {
            product.setBusinessStatus(false);
            productRepository.save(product);
        }
        branchRepository.save(branch);
    }

}
