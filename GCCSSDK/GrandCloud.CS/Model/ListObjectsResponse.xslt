<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" exclude-result-prefixes="xsl">
  <xsl:output method="xml" omit-xml-declaration="no" indent="yes"/>

  <xsl:template match="ListBucketResult">
    <xsl:element name="ListObjectsResponse">
      <xsl:apply-templates/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="Contents">
    <xsl:element name="CSObjects">
      <xsl:element name="BucketName">
        <xsl:value-of select="../Name"/>
      </xsl:element>
      <xsl:apply-templates />
    </xsl:element>
  </xsl:template>

  <xsl:template match="ID">
    <xsl:element name="Id">
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="CommonPrefixes">
    <xsl:element name="CommonPrefixes">
      <xsl:value-of select="./Prefix"/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()"/>
    </xsl:copy>
  </xsl:template>
</xsl:stylesheet>