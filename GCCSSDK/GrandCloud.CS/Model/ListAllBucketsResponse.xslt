<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" exclude-result-prefixes="xsl">
  <xsl:output method="xml" omit-xml-declaration="no" indent="yes"/>
  <xsl:template match="ListAllMyBucketsResult">
    <xsl:element name="ListBucketsResponse">
      <xsl:apply-templates />
    </xsl:element>
  </xsl:template>

  <xsl:template match="Buckets">
    <xsl:apply-templates select="Bucket"/>
  </xsl:template>

  <xsl:template match="Bucket">
    <xsl:element name="Buckets">
      <xsl:element name="BucketName">
        <xsl:value-of select="Name"/>
      </xsl:element>
      <xsl:element name="CreationDate">
        <xsl:value-of select="CreationDate"/>
      </xsl:element>
      <xsl:element name="BucketRegionName">
        <xsl:value-of select="Location"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()"/>
    </xsl:copy>
  </xsl:template>
</xsl:stylesheet>