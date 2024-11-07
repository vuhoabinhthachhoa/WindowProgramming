package com.windowprogramming.ClothingStoreManager.repository.custom.employee;

import com.windowprogramming.ClothingStoreManager.dto.request.employee.EmployeeSearchRequest;
import com.windowprogramming.ClothingStoreManager.dto.request.product.ProductSearchRequest;
import com.windowprogramming.ClothingStoreManager.entity.Employee;
import com.windowprogramming.ClothingStoreManager.entity.Product;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;

public interface CustomEmployeeRepository {
    Page<Employee> searchEmployees(EmployeeSearchRequest request, Pageable pageable);
}
