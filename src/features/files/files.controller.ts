import {
  Controller,
  Delete,
  HttpCode,
  HttpStatus,
  Param,
  Post,
  UploadedFile,
  UseInterceptors,
} from "@nestjs/common";
import { FilesService } from "./files.service";
import { FileInterceptor } from "@nestjs/platform-express";
import { SwaggerFile } from "../../core/decorators/swagger-file.decorator";
import { FileDto } from "../../core/contracts/dto/file.dto";
import { Auth } from "../../core/decorators/auth.decorator";
import {
  ApiBadRequestResponse,
  ApiCreatedResponse,
  ApiNoContentResponse,
  ApiNotFoundResponse,
} from "@nestjs/swagger";
import { ErrorDto } from "../../core/contracts/dto/error.dto";

@Controller("files")
export class FilesController {
  constructor(private readonly filesService: FilesService) {}

  @Post(":folder")
  @Auth()
  @UseInterceptors(FileInterceptor("file"))
  @SwaggerFile(FileDto)
  @ApiCreatedResponse({
    type: FileDto,
    description: "File uploaded successfully",
  })
  @ApiBadRequestResponse({
    description: "File already exists",
    type: ErrorDto,
  })
  public async uploadFile(
    @UploadedFile() file: Express.Multer.File,
    @Param("folder") folder: string,
  ): Promise<string> {
    return await this.filesService.upload(file, folder);
  }

  @Delete(":path")
  @Auth()
  @HttpCode(HttpStatus.NO_CONTENT)
  @ApiNoContentResponse({
    description: "File deleted successfully",
  })
  @ApiNotFoundResponse({
    description: "File not found",
    type: ErrorDto,
  })
  public async deleteFile(@Param("path") path: string): Promise<void> {
    return await this.filesService.delete(path);
  }
}
