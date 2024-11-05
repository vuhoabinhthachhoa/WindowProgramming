package com.windowprogramming.ClothingStoreManager.entity;

import jakarta.persistence.*;
import lombok.*;
import lombok.experimental.FieldDefaults;

@Entity(name = "branches")
@Table(name = "branches")
@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class Branch {
    @Id
    @Column(name = "name", nullable = false)
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
