package com.windowprogramming.ClothingStoreManager.repository.custom.employee;

import com.windowprogramming.ClothingStoreManager.dto.request.employee.EmployeeSearchRequest;
import com.windowprogramming.ClothingStoreManager.dto.request.product.ProductSearchRequest;
import com.windowprogramming.ClothingStoreManager.entity.Employee;
import com.windowprogramming.ClothingStoreManager.entity.Product;
import com.windowprogramming.ClothingStoreManager.utils.IsExistingParamUtils;
import jakarta.persistence.EntityManager;
import jakarta.persistence.TypedQuery;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageImpl;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Repository;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

@Repository
@RequiredArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
public class CustomEmployeeRepositoryImpl implements CustomEmployeeRepository {
    EntityManager entityManager;

    @Override
    public Page<Employee> searchEmployees(EmployeeSearchRequest request, Pageable pageable) {
        StringBuilder jpql = new StringBuilder("SELECT e FROM employees e WHERE 1=1");
        Map<String, Object> parameters = new HashMap<>();

        if (IsExistingParamUtils.isExistingParam(request.getId())) {
            jpql.append(" AND e.id = :id");
            parameters.put("id", "%" + request.getId() + "%");
        }

        if (IsExistingParamUtils.isExistingParam(request.getName())) {
            jpql.append(" AND e.name LIKE :name");
            parameters.put("name", "%" + request.getName() + "%");
        }

        if (IsExistingParamUtils.isExistingParam(request.getPhoneNumber())) {
            jpql.append(" AND e.phoneNumber LIKE :phoneNumber");
            parameters.put("phoneNumber", "%" + request.getPhoneNumber() + "%");
        }

        if (IsExistingParamUtils.isExistingParam(request.getJobTitile())) {
            jpql.append(" AND e.jobTitle LIKE :jobTitle");
            parameters.put("jobTitle", "%" + request.getJobTitile() + "%");
        }

        if (IsExistingParamUtils.isExistingParam(request.getSalaryFrom())) {
            jpql.append(" AND p.salary >= :salaryFrom");
            parameters.put("salaryFrom", request.getSalaryFrom());
        }

        if (IsExistingParamUtils.isExistingParam(request.getSalaryTo())) {
            jpql.append(" AND p.salary <= :salaryTo");
            parameters.put("salaryTo", request.getSalaryTo());
        }

        if (IsExistingParamUtils.isExistingParam(request.getEmploymentStatus())) {
            jpql.append(" AND p.employmentStatus = :employmentStatus");
            parameters.put("employmentStatus", request.getEmploymentStatus());
        }


        // Apply sorting
        if (pageable.getSort().isSorted()) {
            jpql.append(" ORDER BY ");
            for (Sort.Order order : pageable.getSort()) {
                jpql.append("p.").append(order.getProperty()).append(" ").append(order.getDirection()).append(", ");
            }
            jpql.setLength(jpql.length() - 2); // Remove the trailing comma and space
        }

        TypedQuery<Employee> query = entityManager.createQuery(jpql.toString(), Employee.class);
        parameters.forEach(query::setParameter);

        // print the query with real parameter for testing
        System.out.println("Query: " + query.unwrap(org.hibernate.query.Query.class).getQueryString());

        query.setFirstResult((int) pageable.getOffset());
        query.setMaxResults(pageable.getPageSize());

        List<Employee> employees = query.getResultList();

        TypedQuery<Long> countQuery = entityManager.createQuery(
                "SELECT COUNT(p) FROM employees p WHERE 1=1", Long.class);
        Long count = countQuery.getSingleResult();

        return new PageImpl<>(employees, pageable, count);
    }
}
