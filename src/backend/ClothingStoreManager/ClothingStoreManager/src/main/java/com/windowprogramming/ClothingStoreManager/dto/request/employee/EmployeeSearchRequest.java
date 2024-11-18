package com.windowprogramming.ClothingStoreManager.dto.request.employee;

import lombok.AccessLevel;
import lombok.Builder;
import lombok.Getter;
import lombok.Setter;
import lombok.experimental.FieldDefaults;

import java.math.BigDecimal;

@Getter
@Setter
@Builder
@FieldDefaults(level = AccessLevel.PRIVATE)
public class EmployeeSearchRequest {
    // mã, tên, số điện thoại, cccd, chức vụ, lương, employment_status
    Long id;
    String name;
    String phoneNumber;
    String citizenId;
    String jobTitile;
    BigDecimal salaryFrom;
    BigDecimal salaryTo;
    Boolean employmentStatus;
}
