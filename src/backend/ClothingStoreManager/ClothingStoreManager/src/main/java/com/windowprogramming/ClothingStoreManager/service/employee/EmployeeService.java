package com.windowprogramming.ClothingStoreManager.service.employee;

import com.windowprogramming.ClothingStoreManager.dto.request.employee.EmployeeCreationRequest;
import com.windowprogramming.ClothingStoreManager.dto.request.employee.EmployeeSearchRequest;
import com.windowprogramming.ClothingStoreManager.dto.request.employee.EmployeeUpdateRequest;
import com.windowprogramming.ClothingStoreManager.dto.request.product.ProductSearchRequest;
import com.windowprogramming.ClothingStoreManager.dto.response.EmployeeResponse;
import com.windowprogramming.ClothingStoreManager.dto.response.PageResponse;
import com.windowprogramming.ClothingStoreManager.dto.response.ProductResponse;
import com.windowprogramming.ClothingStoreManager.enums.SortType;

import java.util.List;

public interface EmployeeService {
    PageResponse<EmployeeResponse> searchEmployees(EmployeeSearchRequest employeeSearchRequest, Integer page, Integer size, String sortField, SortType sortType);
    EmployeeResponse createEmployee(EmployeeCreationRequest employeeCreationRequest);
    EmployeeResponse getEmployeeById(Long id);
    List<EmployeeResponse> getAllEmployees();
    EmployeeResponse updateEmployee(EmployeeUpdateRequest employeeUpdateRequest);
    void setEmploymentStatusToUnemployed(Long id);
    void setEmploymentStatusesToUnemployed(List<Long> ids);
}
