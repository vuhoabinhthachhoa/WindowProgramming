package com.windowprogramming.ClothingStoreManager.entity;

import jakarta.persistence.*;
import lombok.*;
import lombok.experimental.FieldDefaults;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;


@Entity(name = "invoices")
@Table(name = "invoices")
@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class Invoice {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id")
    Long id;

    @ManyToOne(fetch = FetchType.EAGER)
    @JoinColumn(name = "employee_id", nullable = false)
    Employee employee;

    @Column(name = "created_date", nullable = false)
    LocalDate createdDate;

//    @ManyToOne(fetch = FetchType.EAGER)
//    @JoinColumn(name = "customer_id")
//    Customer customer;
//
//    @ElementCollection
//    @CollectionTable(name = "invoice_vouchers", joinColumns = @JoinColumn(name = "invoice_id"))
//    @Column(name = "voucher_id")
//    List<Long> voucherIds;

    @Column(name = "total_amount", nullable = false, precision = 14, scale = 2)
    BigDecimal totalAmount;

    @Column(name = "real_amount", nullable = false, precision = 14, scale = 2)
    BigDecimal realAmount;
}