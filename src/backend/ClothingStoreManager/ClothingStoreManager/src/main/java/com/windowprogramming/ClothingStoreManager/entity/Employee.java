package com.windowprogramming.ClothingStoreManager.entity;

import jakarta.persistence.*;
import lombok.*;
import lombok.experimental.FieldDefaults;

import java.math.BigDecimal;
import java.time.LocalDate;

@Entity(name = "employees")
@Table(name = "employees")
@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class Employee {
    @Id
    @Column(name = "id")
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    Long id;

    @Column(name = "name", nullable = false)
    String name;

    @Column(name = "phonenumber")
    String phoneNumber;

    @Column(name = "citizenId", nullable = false, unique = true)
    String citizenId;

    @Column(name = "jobTitile",  nullable = false)
    String jobTitile;

    @Column(name = "salary", nullable = false)
    BigDecimal salary;

    @Column(name = "email")
    String email;

    @Column(name = "date_of_birth")
    LocalDate dateOfBirth;

    @Column(name = "address")
    String address;

    @Column(name = "area")
    String area;

    @Column(name = "ward")
    String ward;

}
