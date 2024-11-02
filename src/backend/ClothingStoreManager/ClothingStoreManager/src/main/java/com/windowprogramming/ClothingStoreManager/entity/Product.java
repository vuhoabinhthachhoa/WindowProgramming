package com.windowprogramming.ClothingStoreManager.entity;

import com.windowprogramming.ClothingStoreManager.enums.Size;
import jakarta.persistence.*;
import lombok.*;
import lombok.experimental.FieldDefaults;

import java.math.BigDecimal;
import java.util.List;

@Entity(name = "products")
@Table(name = "products")
@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class Product {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id")
    Long id;

    @Column(name = "code", nullable = false)
    String code;

    @Column(name = "name", nullable = false)
    String name;

    @ManyToOne(fetch = FetchType.EAGER)
    @JoinColumn(name = "category_id", nullable = false)
    Category category;

    @Column(name = "import_price", nullable = false, precision = 14, scale = 2)
    BigDecimal importPrice;

    @Column(name = "selling_price", nullable = false, precision = 14, scale = 2)
    BigDecimal sellingPrice;

    @ManyToOne(fetch = FetchType.EAGER)
    @JoinColumn(name = "branch_id", nullable = false)
    Branch branch;

    @Column(name = "inventory_quantity", nullable = false)
    Short inventoryQuantity;

    @Column(name = "imageUrl")
    String imageUrl;

    @Column(name = "cloudinary_image_id")
    private String cloudinaryImageId;

    @Column(name = "business_status", nullable = false)
    Boolean businessStatus;

    @Column(name = "size", nullable = false)
    Size size;

    @Column(name = "discount_percent", precision = 3, scale = 2)
    BigDecimal discountPercent;
}