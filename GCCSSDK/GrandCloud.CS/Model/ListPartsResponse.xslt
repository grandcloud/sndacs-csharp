<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" exclude-result-prefixes="xsl">
  <xsl:output method="xml" omit-xml-declaration="no" indent="yes"/>

  <xsl:template match="ListPartsResult">
    <ListPartsResponse>
      <BucketName>
        <xsl:value-of select="Bucket"/>
      </BucketName>
      <Key>
        <xsl:value-of select="Key"/>
      </Key>
      <UploadId>
        <xsl:value-of select="UploadId"/>
      </UploadId>
      <StorageClass>
        <xsl:value-of select="StorageClass"/>
      </StorageClass>
      <PartNumberMarker>
        <xsl:value-of select="PartNumberMarker"/>
      </PartNumberMarker>
      <NextPartNumberMarker>
        <xsl:value-of select="NextPartNumberMarker"/>
      </NextPartNumberMarker>
      <MaxParts>
        <xsl:value-of select="MaxParts"/>
      </MaxParts>
      <IsTruncated>
        <xsl:value-of select="IsTruncated"/>
      </IsTruncated>

      <Parts>
        <xsl:apply-templates select="Part"/>
      </Parts>

    </ListPartsResponse>
  </xsl:template>

  <xsl:template match="Part">
    <PartDetail>
      <PartNumber>
        <xsl:value-of select="PartNumber"/>
      </PartNumber>
      <LastModified>
        <xsl:value-of select="LastModified"/>
      </LastModified>
      <ETag>
        <xsl:value-of select="ETag"/>
      </ETag>
      <Size>
        <xsl:value-of select="Size"/>
      </Size>
    </PartDetail>
  </xsl:template>

</xsl:stylesheet>
