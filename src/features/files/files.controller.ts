import {
  Controller,
  Delete,
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

@Controller("files")
export class FilesController {
  constructor(private readonly filesService: FilesService) {}

  @Post(":folder")
  @Auth()
  @UseInterceptors(FileInterceptor("file"))
  @SwaggerFile(FileDto)
  public async uploadFile(
    @UploadedFile() file: Express.Multer.File,
    @Param("folder") folder: string,
  ): Promise<string> {
    return await this.filesService.upload(file, folder);
  }

  @Delete(":path")
  @Auth()
  public async deleteFile(@Param("path") path: string): Promise<void> {
    return await this.filesService.delete(path);
  }
}
