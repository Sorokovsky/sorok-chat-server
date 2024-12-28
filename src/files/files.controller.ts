import {
  Controller,
  Param,
  Post,
  UploadedFile,
  UseInterceptors,
} from "@nestjs/common";
import { FilesService } from "./files.service";
import { FileInterceptor } from "@nestjs/platform-express";
import { SwaggerFile } from "../decorators/swagger-file.decorator";
import { FileDto } from "../contracts/dto/file.dto";

@Controller("files")
export class FilesController {
  constructor(private readonly filesService: FilesService) {}

  @Post(":folder")
  @UseInterceptors(FileInterceptor("file"))
  @SwaggerFile(FileDto)
  public uploadFile(
    @UploadedFile() file: Express.Multer.File,
    @Param("folder") folder: string,
  ) {
    return this.filesService.upload(file, folder);
  }
}
