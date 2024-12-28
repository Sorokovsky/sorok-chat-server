import { Injectable } from "@nestjs/common";
import { join } from "node:path";
import { SERVER_FOLDER } from "../constants/default.constant";
import { writeFile, lstat, mkdir, rm } from "node:fs/promises";
import { Stats } from "node:fs";

@Injectable()
export class FilesService {
  public async upload(
    file: Express.Multer.File,
    folder: string,
    name?: string,
  ): Promise<string> {
    const extension: string = this.getExtension(file.originalname);
    const fileName = name ?? this.getFileName(file.originalname);
    const newFileName: string = `${fileName}.${extension}`;
    const resultPath: string = join(folder, newFileName);
    const serverFolder: string = join(SERVER_FOLDER, folder);
    const serverPath: string = join(serverFolder, newFileName);
    if ((await this.isFolderExist(serverFolder)) === false) {
      await mkdir(serverFolder, { recursive: true });
    }
    await writeFile(serverPath, file.buffer);
    return resultPath;
  }

  public async delete(path: string): Promise<void> {
    const serverPath: string = join(SERVER_FOLDER, path);
    console.log(serverPath);
    return await rm(serverPath, { recursive: true, force: true });
  }

  private getExtension(fileFullName: string): string {
    const parts: string[] = fileFullName.split(".");
    return parts[parts.length - 1];
  }

  private getFileName(fileFullName: string): string {
    const parts: string[] = fileFullName.split(".");
    return parts[0];
  }

  private async isFolderExist(folder: string): Promise<boolean> {
    try {
      const stats: Stats = await lstat(folder);
      return stats.isDirectory();
    } catch {
      return false;
    }
  }
}
