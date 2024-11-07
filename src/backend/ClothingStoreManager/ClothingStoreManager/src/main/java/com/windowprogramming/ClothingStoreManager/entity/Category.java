package com.windowprogramming.ClothingStoreManager.entity;

import jakarta.persistence.*;
import lombok.*;
import lombok.experimental.FieldDefaults;

@Entity(name = "categories")
@Table(name = "categories")
@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class Category {
    @Id
    @Column(name = "id")
    String id;

    @Column(name = "name", nullable = false, unique = true)
    String name;

    @Column(name = "business_status", nullable = false)
    Boolean businessStatus;

    @PrePersist
    public void prePersist() {
        if(this.businessStatus == null) {
            this.businessStatus = true;
        }
    }
}

