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

    // actually, the name is unique, but if we set name as the primary key,
    // we can't update the name of the branch
    @Id
    @Column(name = "id")
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    Long id;

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
