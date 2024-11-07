package com.windowprogramming.ClothingStoreManager.controller;

import com.windowprogramming.ClothingStoreManager.dto.request.employee.EmployeeCreationRequest;
import com.windowprogramming.ClothingStoreManager.dto.request.employee.EmployeeSearchRequest;
import com.windowprogramming.ClothingStoreManager.dto.request.employee.EmployeeUpdateRequest;
import com.windowprogramming.ClothingStoreManager.dto.request.product.ProductSearchRequest;
import com.windowprogramming.ClothingStoreManager.dto.response.ApiResponse;
import com.windowprogramming.ClothingStoreManager.dto.response.EmployeeResponse;
import com.windowprogramming.ClothingStoreManager.dto.response.PageResponse;
import com.windowprogramming.ClothingStoreManager.dto.response.ProductResponse;
import com.windowprogramming.ClothingStoreManager.enums.SortType;
import com.windowprogramming.ClothingStoreManager.service.employee.EmployeeService;
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
@RequestMapping("/employee")
@RequiredArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
@Tag(name = "Employee Controller", description = "APIs for managing employees")
public class EmployeeController {
    EmployeeService employeeService;

    @NonFinal
    @Value("${app.controller.employee.response.unemployment.success}")
    String UNEMPLOYMENT_SUCCESS;


    // search employee

    @PostMapping()
    @Operation(summary = "Create employee",
            description = "Create employee")
    @PreAuthorize("hasAuthority('ADMIN')")
    public ApiResponse<EmployeeResponse> createEmployee(@Valid @RequestBody EmployeeCreationRequest employeeCreationRequest) {
        return ApiResponse.<EmployeeResponse>builder()
                .data(employeeService.createEmployee(employeeCreationRequest))
                .build();
    }

    @GetMapping("/search")
    @Operation(summary = "Search employees", description = "Search for employees with pagination and sorting")
    @PreAuthorize("hasAuthority('ADMIN')")
    public ApiResponse<PageResponse<EmployeeResponse>> searchEmployees(@Valid @RequestBody EmployeeSearchRequest employeeSearchRequest,
                                                                     @RequestParam @NotNull Integer page,
                                                                     @RequestParam @NotNull Integer size,
                                                                     @RequestParam @NotNull String sortField,
                                                                     @RequestParam @NotNull SortType sortType) {
        return ApiResponse.<PageResponse<EmployeeResponse>>builder()
                .data(employeeService.searchEmployees(employeeSearchRequest, page, size, sortField, sortType))
                .build();
    }


    @GetMapping("/all")
    @Operation(summary = "Get all employees",
            description = "Get all employees")
    @PreAuthorize("hasAuthority('ADMIN')")
    public ApiResponse<List<EmployeeResponse>> getAllEmployees() {
        return ApiResponse.<List<EmployeeResponse>>builder()
                .data(employeeService.getAllEmployees())
                .build();
    }

    @GetMapping()
    @Operation(summary = "Get employee by id",
            description = "Get employee by id")
    @PreAuthorize("hasAuthority('ADMIN')")
    public ApiResponse<EmployeeResponse> getEmployeeById(@RequestParam @NotNull Long employeeId) {
        return ApiResponse.<EmployeeResponse>builder()
                .data(employeeService.getEmployeeById(employeeId))
                .build();
    }

    @PatchMapping("status/unemployed")
    @Operation(summary = "Set the employee employment status to unemployed",
            description = "Set the employee employment status to unemployed")
    @PreAuthorize("hasAuthority('ADMIN')")
    public ApiResponse<String> setEmploymentStatusToUnemployed(@RequestParam @NotNull Long employeeId) {
        return ApiResponse.<String>builder()
                .data(UNEMPLOYMENT_SUCCESS)
                .build();
    }

    @PutMapping()
    @Operation(summary = "Update employee",
            description = "Update employee")
    public ApiResponse<EmployeeResponse> updateEmployee(@Valid @RequestBody EmployeeUpdateRequest employeeUpdateRequest) {
        return ApiResponse.<EmployeeResponse>builder()
                .data(employeeService.updateEmployee(employeeUpdateRequest))
                .build();
    }

}
