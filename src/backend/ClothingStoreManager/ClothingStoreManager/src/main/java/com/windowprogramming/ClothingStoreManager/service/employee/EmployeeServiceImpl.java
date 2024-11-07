package com.windowprogramming.ClothingStoreManager.service.employee;

import com.windowprogramming.ClothingStoreManager.dto.request.employee.EmployeeCreationRequest;
import com.windowprogramming.ClothingStoreManager.dto.request.employee.EmployeeSearchRequest;
import com.windowprogramming.ClothingStoreManager.dto.request.employee.EmployeeUpdateRequest;
import com.windowprogramming.ClothingStoreManager.dto.response.EmployeeResponse;
import com.windowprogramming.ClothingStoreManager.dto.response.PageResponse;
import com.windowprogramming.ClothingStoreManager.dto.response.ProductResponse;
import com.windowprogramming.ClothingStoreManager.entity.Employee;
import com.windowprogramming.ClothingStoreManager.entity.Product;
import com.windowprogramming.ClothingStoreManager.enums.SortType;
import com.windowprogramming.ClothingStoreManager.exception.AppException;
import com.windowprogramming.ClothingStoreManager.exception.ErrorCode;
import com.windowprogramming.ClothingStoreManager.mapper.EmployeeMapper;
import com.windowprogramming.ClothingStoreManager.repository.EmployeeRepository;
import com.windowprogramming.ClothingStoreManager.repository.UserRepository;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;

@Service
@RequiredArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
public class EmployeeServiceImpl implements EmployeeService {
    EmployeeRepository employeeRepository;
    UserRepository userRepository;
    EmployeeMapper employeeMapper;

    @Override
    public PageResponse<EmployeeResponse> searchEmployees(EmployeeSearchRequest employeeSearchRequest, Integer page, Integer size, String sortField, SortType sortType) {
        Pageable pageable = PageRequest.of(page - 1, size, sortType == SortType.ASC ? Sort.by(sortField).ascending() : Sort.by(sortField).descending());
        Page<Employee> employees = employeeRepository.searchEmployees(employeeSearchRequest, pageable);
        List<EmployeeResponse> responses = buildEmployeeResponses(employees.getContent());

        return PageResponse.<EmployeeResponse>builder()
                .page(page)
                .size(size)
                .totalElements(employees.getTotalElements())
                .totalPages(employees.getTotalPages())
                .data(responses)
                .build();
    }

    @Override
    public EmployeeResponse createEmployee(EmployeeCreationRequest employeeCreationRequest) {
        // check if employee already exists
        if(employeeRepository.existsByCitizenId(employeeCreationRequest.getCitizenId())){
            throw new AppException(ErrorCode.EMPLOYEE_EXISTED);
        }

        Employee employee = employeeMapper.toEmployee(employeeCreationRequest);
        employee = employeeRepository.save(employee);
        return employeeMapper.toEmployeeResponse(employee);
    }

    @Override
    public EmployeeResponse getEmployeeById(Long id) {
        Employee employee = employeeRepository.findById(id)
                .orElseThrow(() -> new AppException(ErrorCode.EMPLOYEE_NOT_FOUND));
        return employeeMapper.toEmployeeResponse(employee);
    }

    @Override
    public List<EmployeeResponse> getAllEmployees() {
        List<Employee> employees = employeeRepository.findAll();
        List<EmployeeResponse> employeeResponses = new ArrayList<>();
        for(Employee employee : employees) {
            employeeResponses.add(employeeMapper.toEmployeeResponse(employee));
        }
        return employeeResponses;
    }

    @Override
    public EmployeeResponse updateEmployee(EmployeeUpdateRequest employeeUpdateRequest) {
        Employee employee = employeeRepository.findById(employeeUpdateRequest.getId())
                .orElseThrow(() -> new AppException(ErrorCode.EMPLOYEE_NOT_FOUND));
        employeeMapper.updateEmployee(employee, employeeUpdateRequest);
        employee = employeeRepository.save(employee);
        return employeeMapper.toEmployeeResponse(employee);
    }

    @Override
    public void setEmploymentStatusToUnemployed(Long id) {
        Employee employee = employeeRepository.findById(id)
                .orElseThrow(() -> new AppException(ErrorCode.EMPLOYEE_NOT_FOUND));
        employee.setEmploymentStatus(false);
        employeeRepository.save(employee);
    }

    private List<EmployeeResponse> buildEmployeeResponses(List<Employee> employees) {
        List<EmployeeResponse> employeeResponses = new ArrayList<>();
        for(Employee employee : employees) {
            employeeResponses.add(employeeMapper.toEmployeeResponse(employee));
        }
        return employeeResponses;
    }

}
