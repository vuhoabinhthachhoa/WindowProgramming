//package com.windowprogramming.ClothingStoreManager.utils;
//
//import org.hibernate.HibernateException;
//import org.hibernate.engine.spi.SharedSessionContractImplementor;
//import org.hibernate.id.IdentifierGenerator;
//
//import java.sql.Connection;
//import java.sql.PreparedStatement;
//import java.sql.ResultSet;
//import java.sql.SQLException;
//
//public class ProductIdGenerator implements IdentifierGenerator {
//    private String sequenceName;
//
//    public void setSequenceName(String sequenceName) {
//        this.sequenceName = sequenceName;
//    };
//
//    @Override
//    public Object generate(SharedSessionContractImplementor session, Object o) {
//        Connection connection = session.connection();
//        PreparedStatement preparedStatement = null;
//        ResultSet resultSet = null;
//
//        try {
//            preparedStatement = connection.prepareStatement("SELECT NEXTVAL('" + sequenceName + "')");
//            resultSet = preparedStatement.executeQuery();
//            if (resultSet.next()) {
//                return resultSet.getLong(1);
//            } else {
//                throw new HibernateException("Failed to generate ID using sequence " + sequenceName);
//            }
//        } catch (SQLException e) {
//            throw new HibernateException(e);
//        } finally {
//            if (resultSet != null) {
//                try {
//                    resultSet.close();
//                } catch (SQLException e) {
//                    // Ignore
//                }
//            }
//            if (preparedStatement != null) {
//                try {
//                    preparedStatement.close();
//                } catch (SQLException e) {
//                    // Ignore
//                }
//            }
//            if (connection != null) {
//                try {
//                    connection.close();
//                } catch (SQLException e) {
//                    // Ignore
//                }
//            }
//        }
//    }
//    }
//}
