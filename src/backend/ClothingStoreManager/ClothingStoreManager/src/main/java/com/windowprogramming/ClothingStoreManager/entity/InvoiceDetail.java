package com.windowprogramming.ClothingStoreManager.entity;

import jakarta.persistence.*;
import jakarta.validation.constraints.DecimalMax;
import jakarta.validation.constraints.DecimalMin;
import lombok.*;
import lombok.experimental.FieldDefaults;

import java.math.BigDecimal;

@Entity(name = "invoice_details")
@Table(name = "invoice_details")
@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class InvoiceDetail {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id")
    Long id;

    @ManyToOne(fetch = FetchType.EAGER)
    @JoinColumn(name = "invoice_id", nullable = false)
    Invoice invoice;

    @ManyToOne(fetch = FetchType.EAGER)
    @JoinColumn(name = "product_id", nullable = false)
    Product product;

    @Column(name = "import_price", nullable = false, precision = 14, scale = 2)
    BigDecimal importPrice;

    @Column(name = "selling_price", nullable = false, precision = 14, scale = 2)
    BigDecimal sellingPrice;

    @Column(name = "discount_percent", precision = 3, scale = 2)
    @DecimalMin(value = "0.00", message = "Discount percent must be at least 0.00")
    @DecimalMax(value = "1.00", message = "Discount percent must be at most 1.00")
    BigDecimal discountPercent;

    @Column(name = "quantity", nullable = false)
    Short quantity;

    @PrePersist
    public void prePersist() {
        if(this.quantity == null) {
            this.quantity = 1;
        }
        if(this.discountPercent == null) {
            this.discountPercent = BigDecimal.ZERO;
        }
    }


}