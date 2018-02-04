/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50711
Source Host           : localhost:3306
Source Database       : securitycitydb

Target Server Type    : MYSQL
Target Server Version : 50711
File Encoding         : 65001

Date: 2018-02-04 17:22:51
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for sys_config
-- ----------------------------
DROP TABLE IF EXISTS `sys_config`;
CREATE TABLE `sys_config` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `key` int(11) NOT NULL,
  `value` longtext,
  `valueInt` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of sys_config
-- ----------------------------
INSERT INTO `sys_config` VALUES ('1', '100', '0', '0');
INSERT INTO `sys_config` VALUES ('2', '135', '0', '0');
INSERT INTO `sys_config` VALUES ('3', '134', '0', '0');
INSERT INTO `sys_config` VALUES ('4', '133', '0', '0');
INSERT INTO `sys_config` VALUES ('5', '132', '0', '0');
INSERT INTO `sys_config` VALUES ('6', '131', '0', '0');
INSERT INTO `sys_config` VALUES ('7', '130', '0', '0');
INSERT INTO `sys_config` VALUES ('8', '127', '0', '0');
INSERT INTO `sys_config` VALUES ('9', '136', '0', '0');
INSERT INTO `sys_config` VALUES ('10', '126', '0', '0');
INSERT INTO `sys_config` VALUES ('11', '124', '0', '0');
INSERT INTO `sys_config` VALUES ('12', '123', '0', '0');
INSERT INTO `sys_config` VALUES ('13', '122', '0', '0');
INSERT INTO `sys_config` VALUES ('14', '121', '0', '0');
INSERT INTO `sys_config` VALUES ('15', '120', '0', '0');
INSERT INTO `sys_config` VALUES ('16', '102', '0', '30000');
INSERT INTO `sys_config` VALUES ('17', '101', '', '60000');
INSERT INTO `sys_config` VALUES ('18', '125', '0', '0');
INSERT INTO `sys_config` VALUES ('19', '137', '0', '0');
INSERT INTO `sys_config` VALUES ('20', '104', '59', '59');
