package com.windowprogramming.ClothingStoreManager.controller;

import com.windowprogramming.ClothingStoreManager.dto.request.branch.BranchCreationRequest;
import com.windowprogramming.ClothingStoreManager.dto.request.branch.BranchUpdateRequest;
import com.windowprogramming.ClothingStoreManager.dto.response.ApiResponse;
import com.windowprogramming.ClothingStoreManager.dto.response.BranchResponse;
import com.windowprogramming.ClothingStoreManager.service.branch.BranchService;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.tags.Tag;
import jakarta.validation.Valid;
import jakarta.validation.constraints.NotNull;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import lombok.experimental.NonFinal;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.security.config.annotation.method.configuration.EnableMethodSecurity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@EnableMethodSecurity
@RestController
@RequestMapping("/branch")
@RequiredArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
@Tag(name = "Branch Controller", description = "APIs for managing Branch")
public class BranchController {
    BranchService branchService;

    @NonFinal
    @Value("${app.controller.branch.response.stop-business.success}")
    String STOP_BUSINESS_SUCCESS;

    @PostMapping()
    @Operation(summary = "Create branch",
            description = "Create branch")
    @PreAuthorize("hasAuthority('ADMIN')")
    public ApiResponse<BranchResponse> createBranch(@Valid @RequestBody BranchCreationRequest branchCreationRequest) {
        return ApiResponse.<BranchResponse>builder()
                .data(branchService.createBranch(branchCreationRequest))
                .build();
    }

    @GetMapping("/all")
    @Operation(summary = "Get all branches",
            description = "Get all branches which are in business")
    @PreAuthorize("hasAuthority('ADMIN')")
    public ApiResponse<List<BranchResponse>> getAllBranches() {
        return ApiResponse.<List<BranchResponse>>builder()
                .data(branchService.getAllBranches())
                .build();
    }

    @PutMapping()
    @Operation(summary = "Update branch",
            description = "Update branch")
    @PreAuthorize("hasAuthority('ADMIN')")
    public ApiResponse<BranchResponse> updateBranch(@Valid @RequestBody BranchUpdateRequest branchUpdateRequest) {
        return ApiResponse.<BranchResponse>builder()
                .data(branchService.updateBranch(branchUpdateRequest))
                .build();
    }

    @PatchMapping("status/inactive")
    @Operation(summary = "Set the branch business status to inactive",
            description = "Set the branch business status to inactive")
    @PreAuthorize("hasAuthority('ADMIN')")
    public ApiResponse<String> setBusinessStatusToInactive(@RequestParam @NotNull String branchName) {
        branchService.setBusinessStatusToInactive(branchName);
        return ApiResponse.<String>builder()
                .data(STOP_BUSINESS_SUCCESS)
                .build();
    }

//    @GetMapping()
//    @Operation(summary = "Get branch by name",
//            description = "Get branch by name")
//    @PreAuthorize("hasAuthority('ADMIN')")
//    public ApiResponse<BranchResponse> getBranchById(@RequestParam Long id) {
//        return ApiResponse.<BranchResponse>builder()
//                .data(branchService.getBranchById(id))
//                .build();
//    }




}
