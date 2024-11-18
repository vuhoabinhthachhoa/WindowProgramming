package com.windowprogramming.ClothingStoreManager.repository;

import com.windowprogramming.ClothingStoreManager.entity.Employee;
import com.windowprogramming.ClothingStoreManager.repository.custom.employee.CustomEmployeeRepository;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface EmployeeRepository extends JpaRepository<Employee, Long>, CustomEmployeeRepository {

    boolean existsByCitizenId(String citizenId);
}
