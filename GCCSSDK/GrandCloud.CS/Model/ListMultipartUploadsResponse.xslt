<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" exclude-result-prefixes="xsl">
  <xsl:output method="xml" omit-xml-declaration="no" indent="yes"/>

  <xsl:template match="ListMultipartUploadsResult">
    <ListMultipartUploadsResponse>
      <BucketName>
        <xsl:value-of select="Bucket"/>
      </BucketName>
      <KeyMarker>
        <xsl:value-of select="KeyMarker"/>
      </KeyMarker>
      <UploadIdMarker>
        <xsl:value-of select="UploadIdMarker"/>
      </UploadIdMarker>
      <NextKeyMarker>
        <xsl:value-of select="NextKeyMarker"/>
      </NextKeyMarker>
      <NextUploadIdMarker>
        <xsl:value-of select="NextUploadIdMarker"/>
      </NextUploadIdMarker>
      <MaxUploads>
        <xsl:value-of select="MaxUploads"/>
      </MaxUploads>
      <IsTruncated>
        <xsl:value-of select="IsTruncated"/>
      </IsTruncated>
      <Prefix>
        <xsl:value-of select="Prefix"/>
      </Prefix>
      <Delimiter>
        <xsl:value-of select="Delimiter"/>
      </Delimiter>

      <CommonPrefixes>
      <xsl:apply-templates select="CommonPrefixes"/>
      </CommonPrefixes>
      <MultipartUploads>
      <xsl:apply-templates select="Upload"/>
      </MultipartUploads>
  
  
  </ListMultipartUploadsResponse>
  </xsl:template>

  <xsl:template match="CommonPrefixes">
    <string>
      <xsl:value-of select="./Prefix"/>
    </string>
  </xsl:template>  

  <xsl:template match="Upload">
    <MultipartUpload>
      <Key>
        <xsl:value-of select="Key"/>
      </Key>
      <UploadId>
        <xsl:value-of select="UploadId"/>
      </UploadId>
      <StorageClass>
        <xsl:value-of select="StorageClass"/>
      </StorageClass>
      <Initiated>
        <xsl:value-of select="Initiated"/>
      </Initiated> 
  </MultipartUpload>
  </xsl:template>

</xsl:stylesheet>
